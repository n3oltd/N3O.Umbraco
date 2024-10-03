import { FeedbackCustomFieldDefinitionRes, FeedbackCustomFieldType } from "@n3oltd/umbraco-crowdfunding-client";
import { useFormState } from "react-hook-form";

import { FieldError } from "./FieldError";
import { GoalAllocationFieldsProps } from "../types/goalForm";

export function GoalAllocationFields({field, fieldIndex, goalIndex, register, control}: GoalAllocationFieldsProps) {
  const {errors} = useFormState({
    control
  });

  const error = errors.goals ? (errors.goals as any)[goalIndex]?.allocations || {} : {};


  switch (field.type) {
    case FeedbackCustomFieldType.Text:
      return (
        <label className="input__outer" key={`text-${fieldIndex}`}>
          <h4>{field.name}</h4>
          <div
            className="input big"
            style={{
              marginBottom: "12px",
            }}
          >
            <input
              type="text"
              placeholder=""
              {...register(
                `goals.${goalIndex}.allocations.${field.alias}`,
                {
                  required: field.required
                }
              ) }
              maxLength={field.textMaxLength}
            />
          </div>
          {error[field.alias as keyof FeedbackCustomFieldDefinitionRes] && <FieldError message={`${field.name} is required`}/>}
        </label>
      );
    case FeedbackCustomFieldType.Date:
      return (
        <label className="input__outer" key={`date-${fieldIndex}`}>
          <h4>{field.name}</h4>
          <div
            className="setting__date-input big"
            style={{
              marginBottom: "12px",
            }}
          >
            <input
              placeholder="Select Date"
              type="text"
              {...register(
                `goals.${goalIndex}.allocations.${field.alias}`,
                {
                  required: field.required
                }
              )}
              onFocus={(e) => (e.target.type = "date")}
              onBlur={(e) => (e.target.type = "text")}
            />
            <span>
              <img src="../../images/icons/calendar.svg" alt="" />
            </span>
          </div>
          {error[field.alias as keyof FeedbackCustomFieldDefinitionRes] && <FieldError message={`${field.name} is required`}/>}
        </label>
      );
    case FeedbackCustomFieldType.Bool:
      return (
        <div className="setting__date" id="settingDate">
          <label className="checkD">
            <div className="checkD__box">
              <input type="checkbox" id="checkD" {...register(
                  `goals.${goalIndex}.allocations.${field.alias}`,
                  {
                    required: field.required
                  }
                )}/>
              <span></span>
            </div>
            <p className="sm">{field.name}</p>
          </label>
          {error[field.alias as keyof FeedbackCustomFieldDefinitionRes] && <FieldError message={`${field.name} is required`}/>}
        </div>
      );
    default:
      return null;
  }

}