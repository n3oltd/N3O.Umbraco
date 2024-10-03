import { PropertyType } from "@n3oltd/umbraco-crowdfunding-client";

export type PropertyTypeWithGoal = PropertyType & 'GoalEditor'

export type PropConfig = {
  propType: PropertyTypeWithGoal | undefined;
  propAlias: string;
  nested: boolean;
  isOpen: boolean;
};