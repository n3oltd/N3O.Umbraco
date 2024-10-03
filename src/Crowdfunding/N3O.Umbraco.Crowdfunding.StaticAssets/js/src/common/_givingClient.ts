import { GivingClient } from "@n3oltd/umbraco-giving-client";

export const _givingClient = new GivingClient(import.meta.env.PROD ? window.location.origin : undefined);

