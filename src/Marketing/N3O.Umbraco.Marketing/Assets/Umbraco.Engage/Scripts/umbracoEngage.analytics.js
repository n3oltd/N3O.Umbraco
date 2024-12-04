window.umbracoEngage = (function (umbEngage) {
    umbEngage.analytics = (function () {
        (function polyfills() {
            // Element.closest
            // from: https://developer.mozilla.org/en-US/docs/Web/API/Element/closest#Polyfill
            if (!Element.prototype.matches) {
                Element.prototype.matches =
                    Element.prototype.msMatchesSelector ||
                    Element.prototype.webkitMatchesSelector;
            }

            if (!Element.prototype.closest) {
                Element.prototype.closest = function (s) {
                    var el = this;

                    do {
                        if (Element.prototype.matches.call(el, s)) return el;
                        el = el.parentElement || el.parentNode;
                    } while (el !== null && el.nodeType === 1);
                    return null;
                };
            }
        })();

        var state = {
            // Set via init.
            pageviewGuid: null,

            // Version of client side JSON format
            version: 5,

            timeOnPage: {
                start: getTimestamp(),
            },

            scrollDepth: {
                pixels: 0,
                percentage: 0,
            },

            // format: { href: string, timeClicked: Date }
            links: [],

            // Generic events (category/label/action etc)
            events: [],

            // IAnalyticsVideos
            videos: [],

            umbracoForms: [],
        };

        function init(pageviewGuid) {
            state.pageviewGuid = pageviewGuid;

            trackScrollDepth();
            initLinks();
            engagementTracker.init();
            trackVideos();
            trackUmbracoForms();

            document.addEventListener("visibilitychange", function () {
                if (document.visibilityState === "hidden") {
                    sendClientSideData();
                }
            }, false);

            if (typeof umbEngage.init === "function") {
                umbEngage.init();
            }
        }

        var FORM_ACTION = {
            FOCUS: "Focus",
            UNFOCUS_DATA_ENTERED: "UnfocusDataEntered",
            UNFOCUS_NO_DATA_ENTERED: "UnfocusNoDataEntered",
        };

        function trackUmbracoForms() {
            var formElements = [].slice.call(document.querySelectorAll("form")).filter(function (form) {
                var id = getUmbracoFormsId(form);
                var name = getUmbracoFormsName(form);

                return id != null && name != null && !form.hasAttribute("engage-no-tracking");
            });

            formElements.forEach(function (formElement) {
                trackUmbracoFormsForm(formElement);
            });
        }

        function getUmbracoFormsId(form) {
            if (form == null) return null;

            var formIdEl = form.querySelector("input[name='FormId'][type='hidden']");
            if (formIdEl == null) return null;
            return formIdEl.value;
        }

        function getUmbracoFormsName(form) {
            if (form == null) return null;

            var formNameEl = form.querySelector("input[name='FormName'][type='hidden']");
            if (formNameEl == null) return null;
            return formNameEl.value;
        }

        function trackUmbracoFormsForm(form) {
            var formId = getUmbracoFormsId(form);

            var formMeta = loadFormMeta(formId, true) || {
                id: formId,
                name: getUmbracoFormsName(form),
                submitted: false,
                raisedErrors: false,
                timeStart: null,
                timeEnd: null,
                inViewPort: false,
                translated: false, //Decided on element change
                actions: [],
                errors: [],
            };
            
            detectBrowserTranslatedForm(formMeta);
            state.umbracoForms.push(formMeta);

            var fields = [].slice.call(form.querySelectorAll("input,textarea,select")).filter(function (field) {
                return field.type !== "hidden" && field.type !== "button" && field.type !== "submit" && !field.hasAttribute("engage-no-tracking");
            });

            fields.forEach(function (field) {
                trackField(field, formMeta);
            });

            // In newer versions of Umbraco, the submit button on the last page
            // has a data-umb="submit-forms-form", then we know 100% sure this is the last page.
            // Otherwise we can detect the last page (in the default theme) by looking for a specific
            // CSS class on the submit button.
            // Unfortunately there are issues when clients use custom themes and use a different CSS class.
            // In that case we will never be able to detect the last page and thus always consider the form
            // to be a multi page form. So starting now (1.15.0) we will disable multi page detection until
            // we find a more reliable way. This means multi page forms will not work properly but all single page
            // forms do which is the majority.
            var isLastPage = true;

            if (isLastPage) {
                // Add hidden field to form containing our pageview id in order to 
                // connect this form record to the pageview
                var hiddenInput = document.createElement("input");
                hiddenInput.type = "hidden";
                hiddenInput.name = "__umbracoEngageAnalyticsPageviewId";
                hiddenInput.value = state.pageviewGuid;

                var firstInput = form.querySelector("input");

                if (firstInput != null) {
                    form.insertBefore(hiddenInput, firstInput);
                } else {
                    form.appendChild(hiddenInput);
                }
            }

            // Probably related to jQuery validation / aspnet-validation,
            // form.addEventListener("submit") does not trigger but form.onsubmit = function() { ... }
            // still works so we use that.
            var onSubmitFn = form.onsubmit;

            function detectBrowserTranslatedForm(formMeta) {
                //Google Chrome adds translated-ltr or translated-rtl to the body element when the page is translated.
                var bodyElement = document.querySelector('html')
                
                //Edge adds the _msttexthash to the title if translated
                var titleElement = document.querySelector('title')
                
                function onChange(){
                    if (bodyElement && bodyElement.classList.contains('translated-ltr') || bodyElement.classList.contains('translated-rtl')) {
                        formMeta.translated = true;
                    }
                    else if (titleElement && titleElement.hasAttribute('_msttexthash')){
                        formMeta.translated = true;
                    }
                    else{
                        formMeta.translated = false;
                    }
                }
                
                const observer = new MutationObserver(onChange);
                const config = { attributes: true };
                
                observer.observe(bodyElement, config);
                observer.observe(titleElement, config);
            }

            form.onsubmit = function (evt) {
                if (typeof onSubmitFn === "function") {
                    onSubmitFn.apply(form, arguments);
                }

                if (!isLastPage || (evt.submitter != null && evt.submitter.classList.contains("cancel"))) {
                    // User clicked "Previous" or "Next", this is a multi-page form.
                    // Store state for next pageview and remove from state.umbracoForms so we do not
                    // already send it to server.
                    saveFormMeta(formMeta);

                    var idx = state.umbracoForms.indexOf(formMeta);
                    if (idx !== -1) {
                        state.umbracoForms.splice(idx, 1);
                    }
                } else {
                    // Regular submit
                    formMeta.submitted = true;
                    formMeta.timeEnd = Math.floor(+new Date() / 1000);
                }
            }

            trackUmbracoFormsFormInView(formMeta, form);
        }

        function supportsSessionStorage() {
            return window.sessionStorage != null &&
                typeof window.sessionStorage.setItem === "function" &&
                typeof window.sessionStorage.getItem === "function";
        }

        function saveFormMeta(formMeta) {
            if (supportsSessionStorage()) {
                var json = JSON.stringify(formMeta);
                var key = getFormMetaStorageKey(formMeta.id);
                sessionStorage[key] = json;
            }
        }

        function loadFormMeta(id, removeFromStorage) {
            if (supportsSessionStorage()) {
                var key = getFormMetaStorageKey(id);
                var value = sessionStorage[key];

                if (removeFromStorage) {
                    sessionStorage.removeItem(key);
                }

                if (value != null) {
                    try {
                        return JSON.parse(value);
                    } catch (err) {
                        return null;
                    }
                }
            }
        }

        function getFormMetaStorageKey(id) {
            return "umbraco-engage-form-" + id;
        }

        function trackField(field, formMeta) {
            function addAction(action) {
                addFormAction(field, action, formMeta);
            }

            field.addEventListener("focus", function () {
                addAction(FORM_ACTION.FOCUS);
            }, { passive: true });

            field.addEventListener("blur", function () {
                var value = getFieldValue(field);

                if (value == null || value.length === 0) {
                    addAction(FORM_ACTION.UNFOCUS_NO_DATA_ENTERED);
                } else {
                    addAction(FORM_ACTION.UNFOCUS_DATA_ENTERED);
                }
            }, { passive: true });

            function setTimeStart() {
                if (formMeta.timeStart == null) {
                    formMeta.timeStart = Math.floor(+new Date() / 1000);
                }
            }

            field.addEventListener("keyup", setTimeStart, { passive: true, once: true });
            field.addEventListener("change", setTimeStart, { passive: true, once: true });

            field.addEventListener("invalid", function () {
                // We cannot detect the specific type of error, everything is simply a validation error.
                var errorType = "validation";
                addFormError(field, errorType, formMeta);
            }, { passive: true });

            // Observe validation parent class attribute to detect form validation errors
            var validationEl = field.parentNode.querySelector("span[data-valmsg-for]");
            if (validationEl != null) {
                var observer = new MutationObserver(function (mutations) {
                    for (var i = 0; i < mutations.length; i++) {
                        var mutation = mutations[i];

                        if (mutation.type === "attributes" &&
                            mutation.target.className.indexOf("error") > -1 &&
                            mutation.oldValue.indexOf("valid") > -1) {
                            // We cannot detect the specific type of error, everything is simply a validation error.
                            var errorType = "validation";
                            addFormError(field, errorType, formMeta);
                        }
                    }
                });

                var cfg = { attributes: true, attributeFilter: ["class"], attributeOldValue: true };
                observer.observe(validationEl, cfg);
            }
        }

        function getFieldValue(field) {
            if (field == null) return "";
            return field.value;
        }

        function addFormAction(field, action, formMeta) {
            if (formMeta == null) return;

            if (!Array.isArray(formMeta.actions)) {
                formMeta.actions = [];
            }

            var fieldName = getFieldName(field);
            var fieldKey = getFieldKey(field);

            var evt = {
                timestamp: Math.floor(+new Date() / 1000),
                field: fieldName,
                fieldKey: fieldKey,
                action: action,
            };

            formMeta.actions.push(evt);
        }

        function addFormError(field, errorType, formMeta) {
            if (formMeta == null) return;

            formMeta.raisedErrors = true;

            if (!Array.isArray(formMeta.errors)) {
                formMeta.errors = [];
            }

            var fieldName = getFieldName(field);
            var fieldKey = getFieldKey(field);

            var error = {
                timestamp: Math.floor(+new Date() / 1000),
                field: fieldName,
                fieldKey: fieldKey,
                errorType: errorType,
            };

            formMeta.errors.push(error);
        }

        function getFieldName(field) {
            var id = getFieldId(field);
            if (!id) return "<UNKNOWN>";

            var label = document.querySelector("label[for='" + id + "']");
            if (label != null) {
                return label.textContent.replace(/^\s*|[*\s]*$/g, "");
            } else {
                return id;
            }
        }

        function getFieldId(field) {
            var id = field.id;
            if (!id) return null;

            // The datepicker field uses 2 inputs with one having an id of format
            // <GUID>_1 and the label of the field will reference <GUID> in its for attribute.
            return id.replace(/_\d+$/, "");
        }

        function getFieldKey(field) {
            if (field.dataset.umb != null) {
                return field.dataset.umb.replace(/_\d+$/, "");
            } else {
                return getFieldId(field);
            }
        }

        function trackUmbracoFormsFormInView(formMeta, element) {
            if (typeof window.IntersectionObserver !== "function") {
                // IntersectionObserver not available, browser probably too old.
                return;
            }

            function onIntersect(entries) {
                for (var i = 0; i < entries.length; i++) {
                    if (entries[i].isIntersecting) {
                        formMeta.inViewPort = true;

                        // We are only interested if the form was ever in view,
                        // so when this is the case we stop observing.
                        observer.disconnect();

                        break;
                    }
                }
            }

            // Consider this visible when at least 1 pixel is in view.
            // Some forms will never be completely in view because they are too large.
            var options = { threshold: 0 };
            var observer = new IntersectionObserver(onIntersect, options);
            observer.observe(element);
        }

        function initLinks() {
            document.addEventListener("click", onClick);
            document.addEventListener("auxclick", onClick);

            function onClick(evt) {
                // We work with the <a> element closest to the element being clicked.
                // This is necessary to properly recognize clicks on element nested inside an <a>
                // which will trigger the <a> but do not trigger the onClick event for the <a> itself.
                var a = evt.target.closest("a");
                if (a == null || a.href == null || a.href.length === 0) {
                    return;
                }

                if (evt.button !== 0 && evt.button !== 1) {
                    // Only track left or middle button clicks on links
                    return;
                }

                var href = a.href;

                // Link should be one of the following:
                // 1) External (link domain different from current domain)
                // 2) Points to pdf/doc/docx
                // 3) Anchor link (e.g. #header)
                // 4) mailto: or tel: link
                // Anything else will not be tracked.
                var currentDomain = window.location.origin;
                var isExternal = href.indexOf("http") === 0 && href.indexOf(currentDomain) !== 0;
                var isDocument = /\.(?:pdf|doc|docx)(?:\?|$)/i.test(href);
                var isAnchor = href.indexOf("#") === 0 || href.indexOf(currentDomain + "/#") === 0;
                var isMailToOrTel = href.indexOf("mailto:") === 0 || href.indexOf("tel:") === 0;

                var shouldBeTracked = isExternal || isDocument || isAnchor || isMailToOrTel;

                if (shouldBeTracked) {
                    state.links.push({ href: href, timeClicked: new Date() });
                }
            }
        }

        function trackVideos() {
            trackVideoElements();
            trackYouTube();
            trackVimeo();
        }

        var VIDEO_EVENT = {
            AUTOPLAY: "Autoplay",
            PLAY: "Play",
            PAUSE: "Pause",
            RESUME: "Resume",
            ENDED: "Ended",
            SEEK: "Seek",
        };

        var VIDEO_SOURCE = {
            FILE: "File",
            YOUTUBE: "YouTube",
            VIMEO: "Vimeo",
        };

        function trackVideoElements() {
            Object.defineProperty(HTMLMediaElement.prototype, "playing", {
                get: function () {
                    return this.currentTime > 0 && !this.paused && !this.ended && this.readyState > 2;
                }
            });

            var videos = document.querySelectorAll("video");
            if (videos.length === 0) return;

            // Track every video, even if it is not being watched.
            for (var i = 0; i < videos.length; i++) {
                trackVideoElement(videos[i]);
            }
        }

        function makeTimeWatcher(updateElapsedFn) {
            var timeWatchedTs = null;
            var timeWatchedTimer = null;

            function start() {
                stop();

                timeWatchedTs = getTimestamp();

                timeWatchedTimer = setTimeout(function () {
                    update();
                    start();
                }, 1000);
            }

            function stop() {
                if (timeWatchedTimer != null) {
                    update();

                    clearTimeout(timeWatchedTimer);
                    timeWatchedTimer = null;
                    timeWatchedTs = null;
                }
            }

            function update() {
                if (timeWatchedTs != null) {
                    var now = getTimestamp();
                    var elapsedSeconds = Math.floor((now - timeWatchedTs) / 1000);
                    updateElapsedFn(elapsedSeconds);
                    timeWatchedTs = now;
                }
            }

            return {
                start: start,
                stop: stop,
                update: update,
            };
        }

        function trackVideoElement(video) {
            if (video == null) return;

            var videoMeta = {
                name: null,
                source: VIDEO_SOURCE.FILE,
                url: null,

                totalTimeWatchedInSeconds: 0,
                totalLengthInSeconds: 0,
                inViewPort: false,
                watched: false,
                timestamp: Math.floor(+new Date() / 1000),

                events: [],
            };

            state.videos.push(videoMeta);

            function getCurrentTime() {
                return Math.floor(video.currentTime);
            }

            var firstPlay = true;
            var isPaused = false;

            var timeWatcher = makeTimeWatcher(function (elapsedSeconds) {
                videoMeta.totalTimeWatchedInSeconds += elapsedSeconds;
            });

            function onMetadataLoaded() {
                videoMeta.totalLengthInSeconds = Math.floor(video.duration);

                if (video.currentSrc != null && video.currentSrc.length > 0) {
                    videoMeta.url = video.currentSrc;

                    // <video> elements do not have a name, we use the filename instead.
                    var urlSegments = video.currentSrc.split("/");
                    videoMeta.name = urlSegments[urlSegments.length - 1];
                }
            }

            if (video.readyState > 0) {
                // Metadata is immediately available
                onMetadataLoaded();
            } else {
                video.addEventListener("loadedmetadata", onMetadataLoaded, { once: true, passive: true });
            }

            video.addEventListener("play", function () {
                if (firstPlay && video.autoplay) {
                    addVideoEvent(videoMeta, VIDEO_EVENT.AUTOPLAY, getCurrentTime());
                } else if (isPaused) {
                    addVideoEvent(videoMeta, VIDEO_EVENT.RESUME, getCurrentTime());
                } else {
                    addVideoEvent(videoMeta, VIDEO_EVENT.PLAY, getCurrentTime());
                }

                isPaused = false;
                firstPlay = false;

                timeWatcher.start();
            }, { passive: true });

            video.addEventListener("pause", function () {
                var isEnded = video.currentTime === video.duration;
                addVideoEvent(videoMeta, isEnded ? VIDEO_EVENT.ENDED : VIDEO_EVENT.PAUSE, getCurrentTime());
                isPaused = !isEnded;

                timeWatcher.stop();
            }, { passive: true });

            video.addEventListener("seeked", function () {
                addVideoEvent(videoMeta, VIDEO_EVENT.SEEK, getCurrentTime());
            }, { passive: true });

            video.addEventListener("playing", function () {
                videoMeta.watched = true;
            }, { passive: true });

            trackVideoInView(videoMeta, video);
        }

        function addVideoEvent(videoMeta, action, timeInVideo) {
            if (videoMeta == null) return;

            if (!Array.isArray(videoMeta.events)) {
                videoMeta.events = [];
            }

            var evt = {
                timestamp: Math.floor(+new Date() / 1000),
                action: action,
                startTimeInVideoInSeconds: timeInVideo,
            };

            videoMeta.events.push(evt);
        }

        function trackVideoInView(videoMeta, element) {
            if (typeof window.IntersectionObserver !== "function") {
                // IntersectionObserver not available, browser probably too old.
                return;
            }

            function onIntersect(entries) {
                for (var i = 0; i < entries.length; i++) {
                    if (entries[i].isIntersecting) {
                        videoMeta.inViewPort = true;

                        // We are only interested if the video was ever in view,
                        // so when this is the case we stop observing.
                        observer.disconnect();

                        break;
                    }
                }
            }

            var options = { threshold: 1.0 };
            var observer = new IntersectionObserver(onIntersect, options);
            observer.observe(element);
        }

        function trackYouTube() {
            var ytIframes = document.querySelectorAll("iframe[src*='youtube.com'], iframe[src*='youtube-nocookie.com']");
            if (ytIframes.length === 0) return;
            
            //Force enablejsapi=1 on all YouTube iframes for our tracking to work
            ytIframes.forEach(function(iframe) {
                var src = iframe.getAttribute('src');
                if (src && src.indexOf('enablejsapi=1') === -1) {
                    // Append enablejsapi=1 to the src attribute
                    src += (src.indexOf('?') === -1 ? '?' : '&') + 'enablejsapi=1';
                    iframe.setAttribute('src', src);
                }
            });
            
            addYouTubeAPI(function apiReady(ytApi) {
                if (typeof ytApi.Player == "function") {
                    for (var i = 0; i < ytIframes.length; i++) {
                        trackYouTubeVideo(ytIframes[i], ytApi);
                    }
                }
            });
        }

        function trackYouTubeVideo(iframe, ytApi) {
            if (ytApi == null) {
                return addYouTubeAPI(function (api) { trackYouTubeVideo(iframe, api); });
            }

            var videoMeta = {
                name: null,
                source: VIDEO_SOURCE.YOUTUBE,
                url: null,

                totalTimeWatchedInSeconds: 0,
                totalLengthInSeconds: 0,
                inViewPort: false,
                watched: false,
                timestamp: Math.floor(+new Date() / 1000),

                events: [],
            };

            state.videos.push(videoMeta);

            var player;
            var isPaused = false;
            var firstPlay = true;
            var isAutoplay = iframe.src.indexOf("autoplay=1") > -1;

            function getCurrentTime() {
                if (player == null) return 0;
                return Math.floor(player.playerInfo.currentTime);
            }

            var timeWatcher = makeTimeWatcher(function (elapsedSeconds) {
                videoMeta.totalTimeWatchedInSeconds += elapsedSeconds;
            });

            function setMetadata() {
                // Update the video metadata
                videoMeta.totalLengthInSeconds = Math.floor(player.playerInfo.duration);
                videoMeta.url = player.playerInfo.videoUrl;

                if (player.playerInfo && player.playerInfo.videoData) {
                    videoMeta.name = player.playerInfo.videoData.title;
                }
            }

            player = new ytApi.Player(iframe, {
                events: {
                    onReady: function() {
                        setMetadata()
                    },
                    onStateChange: function (evt) {
                        // Fix for lazy loading
                        setMetadata()

                        switch (evt.data) {
                            case -1: // UNSTARTED
                                break;

                            case ytApi.PlayerState.ENDED:
                                addVideoEvent(videoMeta, VIDEO_EVENT.ENDED, getCurrentTime());
                                timeWatcher.stop();
                                break;

                            case ytApi.PlayerState.PLAYING:
                                var currentTime = getCurrentTime();

                                if (firstPlay && isAutoplay) {
                                    addVideoEvent(videoMeta, VIDEO_EVENT.AUTOPLAY, currentTime);
                                } else {
                                    if (isPaused) {
                                        // YouTube API does not send a Seek event, we try to detect it here manually.
                                        var pauseEvents = videoMeta.events.filter(function (evt) {
                                            return evt.action === VIDEO_EVENT.PAUSE;
                                        });

                                        if (pauseEvents.length > 0) {
                                            var lastPause = pauseEvents[pauseEvents.length - 1];
                                            if (lastPause != null && Math.abs(currentTime - lastPause.startTimeInVideoInSeconds) > 1) {
                                                // We resume at a different point than we paused at -> this is a seek.
                                                addVideoEvent(videoMeta, VIDEO_EVENT.SEEK, currentTime);
                                            }
                                        }
                                    }

                                    addVideoEvent(videoMeta, isPaused ? VIDEO_EVENT.RESUME : VIDEO_EVENT.PLAY, currentTime);
                                }

                                isPaused = false;
                                firstPlay = false;
                                videoMeta.watched = true;
                                timeWatcher.start();

                                break;

                            case ytApi.PlayerState.PAUSED:
                                var currentTime = getCurrentTime();

                                var playEvents = videoMeta.events.filter(function (evt) {
                                    return evt.action === VIDEO_EVENT.PLAY || evt.action === VIDEO_EVENT.RESUME;
                                });

                                if (playEvents.length > 0) {
                                    // YouTube API does not send a Seek event, we try to detect it here manually.
                                    var lastPlay = playEvents[playEvents.length - 1];
                                    if (lastPlay != null) {
                                        var now = Math.floor(+new Date() / 1000);

                                        var playbackRate = player.playerInfo && player.playerInfo.playbackRate || 1;
                                        var expectedElapsedTime = playbackRate * (now - lastPlay.timestamp);
                                        var actualElapsedTime = currentTime - lastPlay.startTimeInVideoInSeconds;

                                        var diff = Math.abs(expectedElapsedTime - actualElapsedTime);
                                        var relativeDiff = diff / (expectedElapsedTime || 1);
                                        if (diff > 1 && relativeDiff > 0.1) {
                                            // More than 10% difference -> this is probably a seek.
                                            addVideoEvent(videoMeta, VIDEO_EVENT.SEEK, currentTime);
                                        }
                                    }
                                }

                                addVideoEvent(videoMeta, VIDEO_EVENT.PAUSE, currentTime);
                                isPaused = true;

                                timeWatcher.stop();

                                break;

                            case ytApi.PlayerState.BUFFERING:
                                break;

                            case ytApi.PlayerState.CUED:
                                break;

                            default: break;
                        }
                    }
                }
            });

            trackVideoInView(videoMeta, iframe);
        }

        function addYouTubeAPI(callback) {
            if (typeof window.YT !== "undefined") {
                // YouTube API was already loaded by client
                if (window.YT.loaded) {
                    callback(window.YT);
                } else {
                    window.YT.ready(function () {
                        callback(window.YT);
                    });
                }
            } else {
                // Load API
                var tag = document.createElement('script');
                tag.src = "https://www.youtube.com/iframe_api";
                tag.addEventListener("load", function () {
                    if (typeof window.YT !== "undefined") {
                        window.YT.ready(function () {
                            callback(window.YT);
                        });
                    }
                });

                var firstScriptTag = document.getElementsByTagName('script')[0];
                firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
            }
        }

        function trackVimeo() {
            var vimeoIframes = document.querySelectorAll("iframe[src*='player.vimeo.com/video']");
            if (vimeoIframes.length === 0) return;

            addVimeoSDK(function sdkReady(vimeoSdk) {
                if (typeof vimeoSdk.Player == "function") {
                    for (var i = 0; i < vimeoIframes.length; i++) {
                        trackVimeoVideo(vimeoIframes[i], vimeoSdk);
                    }
                }
            });
        }

        function trackVimeoVideo(iframe, vimeoSdk) {
            if (vimeoSdk == null) {
                return addVimeoSDK(function (sdk) { trackVimeoVideo(iframe, sdk); });
            }

            var videoMeta = {
                name: null,
                source: VIDEO_SOURCE.VIMEO,
                url: null,

                totalTimeWatchedInSeconds: 0,
                totalLengthInSeconds: 0,
                inViewPort: false,
                watched: false,
                timestamp: Math.floor(+new Date() / 1000),

                events: [],
            };

            state.videos.push(videoMeta);

            var player;
            var isPaused = false;
            var firstPlay = true;
            var isAutoplay = iframe.src.indexOf("autoplay=1") > -1;

            var timeWatcher = makeTimeWatcher(function (elapsedSeconds) {
                videoMeta.totalTimeWatchedInSeconds += elapsedSeconds;
            });

            player = new vimeoSdk.Player(iframe);

            player.getVideoTitle().then(function (title) { videoMeta.name = title; });
            player.getVideoUrl().then(function (url) { videoMeta.url = url; }, function onError() { videoMeta.url = iframe.src.replace(/\?.*$/, ''); });
            player.getDuration().then(function (duration) { videoMeta.totalLengthInSeconds = Math.floor(duration); });

            player.on("play", function (data) {
                var currentTime = Math.floor(data.seconds);

                if (firstPlay && isAutoplay) {
                    addVideoEvent(videoMeta, VIDEO_EVENT.AUTOPLAY, currentTime);
                } else {
                    addVideoEvent(videoMeta, isPaused ? VIDEO_EVENT.RESUME : VIDEO_EVENT.PLAY, currentTime);
                }

                isPaused = false;
                firstPlay = false;
                videoMeta.watched = true;

                timeWatcher.start();
            });

            player.on("pause", function (data) {
                var currentTime = Math.floor(data.seconds);
                addVideoEvent(videoMeta, VIDEO_EVENT.PAUSE, currentTime);
                isPaused = true;

                timeWatcher.stop();
            });

            player.on("ended", function (data) {
                var currentTime = Math.floor(data.seconds);
                addVideoEvent(videoMeta, VIDEO_EVENT.ENDED, currentTime);
                timeWatcher.stop();
            });

            player.on("seeked", function (data) {
                var currentTime = Math.floor(data.seconds);
                addVideoEvent(videoMeta, VIDEO_EVENT.SEEK, currentTime);
            });

            trackVideoInView(videoMeta, iframe);
        }

        function addVimeoSDK(callback) {
            if (typeof window.Vimeo !== "undefined") {
                // Vimeo SDK was already loaded by client
                callback(window.Vimeo);
            } else {
                // Load SDK
                var script = document.createElement("script");
                script.src = "https://player.vimeo.com/api/player.js";

                script.addEventListener("load", function () {
                    if (typeof window.Vimeo !== "undefined") {
                        callback(window.Vimeo);
                    }
                });

                // Important: Vimeo SDK should be loaded after all iframe elements
                // so we put it as last child in the <body> element.
                document.body.appendChild(script);
            }
        }

        function getTimestamp() {
            return (performance && typeof performance.now === "function" && performance.now()) || +new Date();
        }

        function getTimeOnPage() {
            var end = getTimestamp();

            var totalTimeMillis = Math.floor(end - state.timeOnPage.start);
            var engagedTimeMillis = engagementTracker.getTimeEngaged();

            return {
                totalTimeMillis: totalTimeMillis,
                engagedTimeMillis: engagedTimeMillis
            }
        }

        function getScrollDepth() {
            return {
                pixels: state.scrollDepth.pixels,
                percentage: state.scrollDepth.percentage,
            }
        }

        function getLinks() {
            return state.links;
        }

        function getEvents() {
            return state.events;
        }

        function getUmbracoForms() {
            return state.umbracoForms;
        }

        function getClientSideData() {
            var timeOnPage = getTimeOnPage();
            var scrollDepth = getScrollDepth();
            var links = getLinks();
            var events = getEvents();
            var videos = getVideos();
            var umbracoForms = getUmbracoForms();

            return {
                timeOnPage: timeOnPage,
                scrollDepth: scrollDepth,
                links: links,
                events: events,
                umbracoForms: umbracoForms,
                videos: videos,
            }
        }

        function getVideos() {
            return state.videos;
        }

        function sendClientSideData() {
            if (state.pageviewGuid == null) {
                return;
            }

            var data = getClientSideData();
            var url = "/umbraco/engage/pagedata/collect?version=" + state.version + "&pageviewGuid=" + state.pageviewGuid;

            postJSON(url, data);
            resetClientSideData();
        }

        function postJSON(url, data) {
            // https://developer.mozilla.org/en-US/docs/Web/API/Navigator/sendBeacon
            if (typeof navigator.sendBeacon === "function" && typeof Blob === "function") {
                var headers = { type: "application/json" };
                var blob = new Blob([JSON.stringify(data)], headers);
                navigator.sendBeacon(url, blob);
            } else {
                var xhr = new XMLHttpRequest();
                xhr.open('POST', url, false);
                xhr.setRequestHeader("Content-Type", "application/json");
                var payload = JSON.stringify(data);
                xhr.send(payload);
            }
        }

        /**
         * Removes any data that should only be sent to the server once such as events.
         */
        function resetClientSideData() {
            state.links.length = 0;
            state.events.length = 0;

            // state.videos itself cannot be cleared as it contains a "totalTimeWatchedInSeconds" variable that is
            // being continuously updated. The events of this video however can be cleared
            for (var vi = 0; vi < state.videos.length; vi++) {
                state.videos[vi].events.length = 0;
            }

            // The same is true for state.umbracoForms except it does not have events but "actions" and "errors".
            for (var fi = 0; fi < state.umbracoForms.length; fi++) {
                var uf = state.umbracoForms[fi];
                uf.actions.length = 0;
                uf.errors.length = 0;
            }
        }

        function debounce(func, wait) {
            var timeout;
            return function () {
                var context = this;
                var args = arguments;
                var later = function () {
                    timeout = null;
                    func.apply(context, args);
                };
                clearTimeout(timeout);
                timeout = setTimeout(later, wait);
            };
        };

        function trackScrollDepth() {
            window.addEventListener("scroll", debounce(recordScrollDepth, 100), false);
            document.addEventListener("DOMContentLoaded", recordScrollDepth);
            recordScrollDepth();

            function recordScrollDepth() {
                var documentHeight = document.documentElement.offsetHeight;
                var scrollPosition = window.pageYOffset || document.documentElement.scrollTop;
                var viewportHeight = window.innerHeight;
                var viewportBottom = viewportHeight + scrollPosition;

                var percentage = Math.round((Math.min(viewportBottom, documentHeight) / documentHeight) * 100);

                if (percentage > state.scrollDepth.percentage) {
                    state.scrollDepth.percentage = percentage;
                }

                if (viewportBottom > state.scrollDepth.pixels) {
                    state.scrollDepth.pixels = Math.floor(viewportBottom);
                }
            }
        }

        function trackEvent(fields) {
            state.events.push(fields);

            // Update visitor profile on server immediately
            var url = "/umbraco/engage/pagedata/collectevent";
            postJSON(url, fields);
        }

        var engagementTracker = (function () {
            var startEngage = now();
            var timeEngaged = 0;
            var idle = true;
            var idleTimer;

            function now() {
                return new Date().getTime();
            }

            function setIdle() {
                updateTimeEngaged();
                idle = true;
            };

            function pulse() {
                if (idle) {
                    idle = false;
                    startEngage = now();
                }
                window.clearTimeout(idleTimer);
                idleTimer = window.setTimeout(setIdle, 5000);
            };

            function addListener(evt, cb) {
                if (window.addEventListener) {
                    window.addEventListener(evt, cb);
                } else if (window.attachEvent) {
                    window.attachEvent('on' + evt, cb);
                }
            };

            function updateTimeEngaged() {
                if (idle) {
                    return;
                }

                var endEngage = now();

                timeEngaged += endEngage - startEngage;
                startEngage = endEngage;
            }

            function init() {
                addListener('mousedown', pulse);
                addListener('keydown', pulse);
                addListener('scroll', pulse);
                addListener('mousemove', pulse);
                idleTimer = window.setTimeout(setIdle, 5000);
            }

            function getTimeEngaged() {
                updateTimeEngaged();
                return timeEngaged;
            }

            return {
                init: init,
                getTimeEngaged: getTimeEngaged,
            };
        })();

        return {
            init: init,
            trackEvent: trackEvent,
            getClientSideData: getClientSideData,

            video: {
                trackVideoElement: trackVideoElement,
                trackYouTubeVideo: trackYouTubeVideo,
                trackVimeoVideo: trackVimeoVideo,
            }
        };
    })();

    return umbEngage;
})(window.umbracoEngage || {});

window.umbEngage = window.umbEngage || (function () {
    var api = (function init() {
        function sendEvent(fields) {
            fields.timestamp = parseInt(+new Date() / 1000, 10);
            umbracoEngage.analytics.trackEvent(fields);
        }

        return {
            event: {
                send: sendEvent
            }
        };
    })();

    function eventArgumentParser(args) {
        var fields = {};
        for (var i = 0; i < args.length; i++) {
            var arg = args[i];

            if (typeof arg === 'string') {
                switch (i) {
                    case 0: break; // command => skip
                    case 1: fields.hitType = arg; break; // hitType
                    case 2: fields.category = arg; break; // category
                    case 3: fields.action = arg; break; // action
                    case 4: fields.label = arg; break; // label
                }
            } else if (typeof arg === 'number') {
                // This is the only number field type we support
                fields.value = arg;
            } else if (typeof arg === 'object') {
                // fieldsObject is always the last one to be passed
                Object.assign(fields, arg);
                break;
            }
        }
        return fields;
    }

    var hitTypeServiceMap = {
        'event': {
            argumentParser: eventArgumentParser,
            api: function () { return api.event; }
        }
        // Currently not (yet) supported:
        // ,'exception': exceptionHandler
        // ,'item': itemHandler
        // ,'pageview': pageviewHandler
        // ,'social': socialhandler
        // ,'screenview': screenviewHandler
        // ,'timing': timingHandler
        // ,'transaction': transactionHandler
    };

    var onSendEventCallbacks = [];

    function onSendEvent(callback) {
        if(typeof callback !== "function") {
            throw new Error("umbEngage.onSendEvent(callback): callback should be a function");
        }

        onSendEventCallbacks.push(callback);

        return function unsubscribe() {
            var idx = onSendEventCallbacks.indexOf(callback);
            if(idx > -1) {
                onSendEventCallbacks.splice(idx, 1);
            }
        }
    }

    function emitSendEventCancellable(fields) {
        var canceled = false;
        var eventArgs = {
            cancel: function() {
                canceled = true;
            },

            fields: fields
        };

        for(var i = 0; i < onSendEventCallbacks.length; i++) {
            onSendEventCallbacks[i](eventArgs);
        }

        return canceled;
    }

    // Expose the umbEngage API as a function just like Google Analytics' analytics.js API
    function umbEngage (command, hitType) {
        if (typeof command !== 'string' || hitType === '') return;

        var args = Array.prototype.slice.call(arguments);

        if (Object.prototype.toString.call(hitType) === "[object Object]") {
            // When "hitType" is an object, this is the "fieldObject" argument used with this syntax:
            //
            // ga("send", {
            //     hitType: "event",
            //     eventCategory: "category",
            //     ...
            // })
            //
            // We will transform this to the "simple" syntax where each parameter is a separate argument.

            var category = hitType.eventCategory;
            var action = hitType.eventAction;
            var label = hitType.eventLabel;
            var value = hitType.eventValue;

            hitType = hitType.hitType;

            // Push the arguments into a flat array so it can be processed in the same way as
            // the syntax ga("send", "event", "category", "action", "label", value)
            args = [command, hitType, category, action, label, value];
        }

        if (typeof hitType !== 'string' || hitType === '') return;

        var service = hitTypeServiceMap[hitType];

        if (!service) return;

        var fields = service.argumentParser(args);

        var cancel = emitSendEventCancellable(fields);
        if(cancel) {
            // User defined callback specified command should be canceled.
            return;
        }

        var api = service.api();

        var apiCommand = null;

        for (var key in api) {
            if (!api.hasOwnProperty(key)) continue;

            // Find the API that matches the command.
            // We match case insensitive either exactly the key (e.g. "send")
            // or the key prefixed by a custom tracker (e.g. "gtm.send").
            // The custom tracker (if any) will be stored in capture group 1.
            var re = new RegExp("^(?:([^.]+)\\.)?" + key + "$", "i");
            if (re.test(command)) {
                apiCommand = api[key];
                break;
            }
        }

        if (apiCommand) {
            try {
                apiCommand(fields);
            } catch (ex) {
                // For now, silently ignore
            }
        }
    };

    umbEngage.onSendEvent = onSendEvent;

    return umbEngage;
})();
