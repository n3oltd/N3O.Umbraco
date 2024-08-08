import { CrowdfundingClient } from "@n3oltd/umbraco-crowdfunding-client";

export const _client = new CrowdfundingClient(import.meta.env.PROD ? window.location.origin : undefined);

