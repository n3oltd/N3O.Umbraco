# N3O Umbraco Extensions
[![NuGet](https://img.shields.io/nuget/v/N3O.Umbraco.Extensions)](https://www.nuget.org/packages/N3O.Umbraco.Extensions/)
[![npm](https://img.shields.io/npm/v/@n3oltd/umbraco-giving-client)](https://www.npmjs.com/package/@n3oltd/umbraco-giving-client)
[![Main Branch CI](https://github.com/n3oltd/N3O.Umbraco/actions/workflows/main-ci.yml/badge.svg)](https://github.com/n3oltd/N3O.Umbraco/actions/workflows/main-ci.yml)
[![Tag CI](https://github.com/n3oltd/N3O.Umbraco/actions/workflows/tag-ci.yml/badge.svg)](https://github.com/n3oltd/N3O.Umbraco/actions/workflows/tag-ci.yml)
[![License](https://img.shields.io/github/license/n3oltd/N3O.Umbraco)](LICENSE.md)

This repo contains the N3O Umbraco extensions. These are a series of NuGet packages that extend the functionality of the [Umbraco CMS](https://umbraco.com). As well as providing generic functionality such as payment processing and site search, they provide ready made integration with the [Enagage CRM](https://n3o.ltd/).

Extensions work with both cloud and on-premises versions of Umbraco 10 or later.

Aside from the back office plugins and content apps, extensions do not provide any HTML, CSS or other UI. They are designed to integrate seamlessly with any standard Umbraco template and allow your site to retain a distinctive look and feel.

## Demo Site
The solution includes a demo site that both illustrates much of the functionality and also provides a useful starting point for building new sites. The easiest way to get started is to run the `DemoSite.Web` project in the solution, follow the Umbraco install wizard and then navigate to *Settings / uSync*. From here import *Settings* first, followed by *Content*. This will create all of the demo data types, content types and demo content.

## Functionality
#### Data
Provides a generic and extendable data export/import framework as well as content apps for importing and exporting data from the Umbraco backoffice.

#### Donations
Provides a pluggable architecture for a shopping cart, checkout and receipts.

#### Email
Provides a pluggable architecture for email delivery such as via SMTP or SendGrid.

#### Foreign Exchange
Provides realtime currency conversion using market data.

#### Newsletters
Provides functionality for integrating with newsletter platforms such as Mailchimp.

#### Payments
Provides support for taking one-off and recurring payments using various gateways. Currently supported gateways include:

* Authorize.net
* Bambora
* GoCardless
* Opayo
* PayPal
* Stripe

#### Plugins
Provides a number of back office extensions such as:

* File Uploader
* Image Cropper
* SERP Entry Editor
* Text Resource Editor

These either provide richer functionality than Umbraco includes or work around limitations (e.g. the built in Umbraco cropper doesn't work with Nested Content or Blocks).

#### Search
Provides pluggable implementations for providing site indexing and search functionality using either Google Programmable Search Engine, Algolia or Examine.

#### Tax Relief
Provides pluggable implementations for managing charitable tax relief, such as the UK Gift Aid scheme.

#### Thanks

A big [#h5yr](https://community.umbraco.com/learn-about-the-community/h5yr/) to:

* Arnold Visser for [Our.Umbraco.GMaps](https://github.com/ArnoldV/Our.Umbraco.GMaps)
* Dan Diplo for [Umbraco.GodMode](https://github.com/DanDiplo/Umbraco.GodMode)
* Kevin Jump for [uSync](https://jumoo.co.uk/usync/)
* Lee Kelleher for [Contentment](https://github.com/leekelleher/umbraco-contentment)
* Outfield Digital for [Konstrukt](https://getkonstrukt.net/)
* Perplex Digital for [Perplex.ContentBlocks](https://github.com/PerplexDigital/Perplex.ContentBlocks)

## Questions & Support
For questions and support please visit the [N3O Support Centre](https://support.n3o.ltd/).

## License
All extensions are licensed under the [MIT](LICENSE.md) license.
