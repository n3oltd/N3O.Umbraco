import { PropertyType } from "@n3oltd/umbraco-crowdfunding-client";

export type PropConfig = {
  selector: string;
  propType: PropertyType | undefined;
  propAlias: string;
  nested: boolean;
  isOpen: boolean;
};