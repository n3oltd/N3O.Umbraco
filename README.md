# N3O Umbraco Extensions
[![NuGet](https://img.shields.io/nuget/v/N3O.Umbraco.Extensions)](https://www.nuget.org/packages/N3O.Umbraco.Extensions/) [![Build](https://img.shields.io/github/workflow/status/n3oltd/N3O.Umbraco/main-ci)](../../actions/workflows/main-ci.yml) [![License](https://img.shields.io/github/license/n3oltd/N3O.Umbraco)](LICENSE.md)

This repo contains the N3O Umbraco extensions. These are a series of NuGet packages that extend the functionality of the [Umbraco CMS](https://umbraco.com). As well as providing generic functionality such as payment processing and site search, they provide ready made integration with the [Enagage CRM](https://n3o.ltd/).

Extensions work with both cloud and on-premises versions of Umbraco 9 or later.

Aside from the back office plugins, extensions do not provide any HTML, CSS or other UI. They are designed to integrate seamlessly with any standard Umbraco template and allow your site to retain a distinctive look and feel.

## Functionality
#### Email
Provides a pluggable architecture for email delivery such as via SMTP or SendGrid.

#### Foreign Exchange
Provides realtime currency conversion using market data.

#### Newsletters
Provides functionality for integrating with newsletter platforms such as Mailchimp.

#### Payments
Provides support for taking one-off and recurring payments using various gateways. Currently supported gateways include:

* GoCardless
* PayPal
* Stripe

#### Plugins
Provides a number of back office extensions such as:

* File Uploader
* Image Cropper
* SERP Entry Editor

These either provide richer functionality than Umbraco includes or works around limitations (e.g. the built in Umbraco cropper doesn't work with nested content or blocks).

#### Search
Provides functionality for providing a site search function using Google Custom Search.

#### Tax Relief
Provides pluggable implementations for managing charitable tax relief, such as the UK Gift Aid scheme.

## Questions & Support
For questions and support please visit the [N3O Support Centre](https://support.n3o.ltd/).

## License
All extensions are licensed under the [MIT](LICENSE.md) license.
