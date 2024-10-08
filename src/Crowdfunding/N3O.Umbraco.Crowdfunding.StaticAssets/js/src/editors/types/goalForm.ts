import { FeedbackCustomFieldDefinitionRes, FundraiserGoalsRes, GoalOptionRes, PricingRuleRes } from "@n3oltd/umbraco-crowdfunding-client";

export type GoalPricingRef = Record<string, PricingRuleRes>

export type GoalSchema = {
  goalId: string | undefined;
  amount: number | undefined;
  currency: string | undefined;
  dimension1: string | undefined,
  dimension2: string | undefined,
  dimension3: string | undefined,
  dimension4: string | undefined,
  allocations?: Record<string, string | boolean>;
};

export type FormSchema = {
  goals: GoalSchema[];
};

export type GoalFieldsProps = {
  control: any;
  register: any;
  setValue: any;
  getValues: any;
  remove: (index: number) => void;
  index: number;
  data: FundraiserGoalsRes | undefined,
  pricingRef: any
  availableGoals: GoalOptionRes[]
};

export type GoalAllocationFieldsProps = {
  field: FeedbackCustomFieldDefinitionRes;
  fieldIndex: number;
  goalIndex: number;
  register: any;
  control: any
};
