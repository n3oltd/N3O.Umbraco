import React from "react";

import { useRequest } from "ahooks";
import { useFormState, useWatch } from "react-hook-form";
import { GoalOptionRes } from "@n3oltd/umbraco-crowdfunding-client";

import { FieldError } from "./FieldError";
import { GoalAllocationFields } from "./GoalAllocationFields";

import { n3o_cdf_filterList, selectFunc } from "@/helpers/goal.helpers";
import { _givingClient } from "@/common/_givingClient";
import { GoalFieldsProps } from "../types/goalForm";

const dimensions = [
  'dimension1',
  'dimension2',
  'dimension3',
  'dimension4'
]

export function GoalFields(props: GoalFieldsProps) {
  const {errors} = useFormState({
    control: props.control
  })

  const watchedGoals = useWatch({
    control: props.control,
    name: "goals",
  });

  const { data: pricing,  runAsync: getPricing} = useRequest(criteria => _givingClient.getPrice(criteria), {
    manual: true,
  });

  const selectedGoal : GoalOptionRes | undefined = props.data?.goalOptions?.find(
    (g) => g.id === watchedGoals[props.index]?.goalId
  );

  const {getValues, setValue, index, pricingRef} = props;
  const values = getValues()
  const goal = values.goals[index];
  const {dimension1, dimension2, dimension3, dimension4} = goal;

  React.useEffect(() => {
    const values = getValues()
    const goal = values.goals[index];

    selectFunc();
    
    const criteria = {
      donationItem: selectedGoal?.fund?.id,
      feedbackScheme: selectedGoal?.feedback?.id,
      fundDimensions: {
        dimension1: goal.dimension1,
        dimension2: goal.dimension2,
        dimension3: goal.dimension3,
        dimension4: goal.dimension4,
      }
    }

    getPricing(criteria).then(res => pricingRef.current = {
      [goal.goalId]: res
    });
  }, [getValues, index, selectedGoal, getPricing, pricingRef, dimension1, dimension2, dimension3, dimension4]);


  React.useEffect(()=>{
    if (!selectedGoal){
      return;
    }

    dimensions.forEach((key) => {
      if (goal[key as keyof GoalOptionRes]) {
        return;
      }

      setValue(`goals.${index}.${key}`, (selectedGoal as any)[key]?.default?.id || (selectedGoal as any)[key]?.allowedOptions?.[0]?.id);
    
    });
    
  },[index, selectedGoal, setValue, goal]);

  const goalErrors = (errors.goals as any)?.[props.index];

  return (
    <div className="editGoal__body">
      {props.index > 0 && (
        <button
          type="button"
          onClick={() => props.remove(props.index)}
          className="editGoal__remove"
        >
          <span>
            <img src="/assets/images/icons/x-circle.svg" alt="" />
          </span>
          {window.themeConfig.text.crowdfunding.remove}
        </button>
      )}
      <h4>{window.themeConfig.text.crowdfunding.selectGoal}</h4>
      <div className="select__outer">
        <div className="select">
          <div className="select__selected" id="select__selected"> {selectedGoal?.name || window.themeConfig?.text?.crowdfunding?.selectGoal}</div>
          <input
            type="hidden"
            {...props.register(`goals.${props.index}.goalId`, {
              required: window.themeConfig.text.crowdfunding.goalProjectRequired,
            })}
          />
          <ul className="select__options">
            <div className="select__search">
              <svg>
                <use xlinkHref="#searchIcon"></use>
              </svg>
              <input
                type="text"
                id="gaols-input"
                placeholder={window.themeConfig.text.crowdfunding.searchHerePlaceholder}
                onChange={() => n3o_cdf_filterList('gaols-input', 'goal-options')}
              />
            </div>
            <div className="select__options-body" id="goal-options">
              {props.availableGoals?.map((goal, gIndex) => (
                <li
                  key={gIndex}
                  value={goal.id}
                  onClick={() => {
                    props.setValue(`goals.${props.index}.goalId`, goal.id);
                  }}
                >
                  {goal.name}
                </li>
              ))}
            </div>
          </ul>
          <svg>
            <use xlinkHref="#chevron-bottom"></use>
          </svg>
        </div>
        {goalErrors?.goalId && <FieldError message={goalErrors?.goalId?.message}></FieldError>}
        <div className="setting__tags">
          {props.data?.goalOptions?.find(g => g.id === selectedGoal?.id)?.tags?.map((t: any) => {
            return <a href="#" className="setting__tag" key={t.name}>
            <img  src={t.iconUrl} width="16px" height="16px"/>
            <b> {t.name}</b>
          </a>
          })}
          
        </div>
      </div>

      {(selectedGoal?.dimension1?.allowedOptions as any[])?.length > 1 ? <>
        <h4 className="dimension-1" >{window.themeConfig.text.crowdfunding.goalSelectDimension1}</h4>
        <div className="select__outer dimension-1"  id="dimension-1">
            <div className="select">
                <div className="select__selected">{selectedGoal?.dimension1?.allowedOptions?.find(d => d.id === goal?.dimension1)?.name || window.themeConfig.text.crowdfunding.goalSelectDimension}</div>

                <input type="hidden" {...props.register(`goals.${props.index}.dimension1`)}/>

                <ul className="select__options">
                    <div className="select__search">

                        <svg>
                            <use xlinkHref="#searchIcon"></use>
                        </svg>

                        <input type="text" id="goals-dimension-1" placeholder={window.themeConfig.text.crowdfunding.searchHerePlaceholder} onKeyUp={() => n3o_cdf_filterList('goals-dimension-1', 'goal-selection-dimension-1')}/>
                    </div>

                    <div className="select__options-body" id="goal-selection-dimension-1">
                      {selectedGoal?.dimension1?.allowedOptions?.map((dimension, dIndex) => (
                        <li
                          key={dIndex}
                          value={dimension.id}
                          onClick={() => {
                            props.setValue(`goals.${props.index}.dimension1`, dimension.id);;
                          }}
                        >
                          {dimension.name}
                        </li>
                      ))}
                    </div>
                </ul>

                <svg>
                    <use xlinkHref="#chevron-bottom"></use>
                </svg>
            </div>
        </div>
      </> 
      : <input type="hidden" 
        {...props.register(`goals.${props.index}.dimension1`)}
      />     
      }
      {(selectedGoal?.dimension2?.allowedOptions as any[])?.length > 1 ? <>
        <h4 className="dimension-2" > {window.themeConfig.text.crowdfunding.goalSelectDimension2}</h4>
        <div className="select__outer dimension-2"  id="dimension-2">
            <div className="select">
                <div className="select__selected">{selectedGoal?.dimension2?.allowedOptions?.find(d => d.id === goal?.dimension2)?.name || window.themeConfig.text.crowdfunding.goalSelectDimension}</div>

                <input type="hidden" defaultValue="John Doe"  {...props.register(`goals.${props.index}.dimension2`)}/>

                <ul className="select__options">
                    <div className="select__search">

                        <svg>
                            <use xlinkHref="#searchIcon"></use>
                        </svg>

                        <input type="text" id="goals-dimension-2" placeholder={window.themeConfig.text.crowdfunding.searchHerePlaceholder} onKeyUp={() => n3o_cdf_filterList('goals-dimension-2', 'goal-selection-dimension-2')}/>
                    </div>

                    <div className="select__options-body" id="goal-selection-dimension-2">
                      {selectedGoal?.dimension2?.allowedOptions?.map((dimension, dIndex) => (
                          <li
                            key={dIndex}
                            value={dimension.id}
                            onClick={() => {
                              props.setValue(`goals.${props.index}.dimension2`, dimension.id);
                            }}
                          >
                            {dimension.name}
                          </li>
                        ))}
                    </div>
                </ul>

                <svg>
                    <use xlinkHref="#chevron-bottom"></use>
                </svg>
            </div>
        </div>
      </> 
      : <input type="hidden" 
        {...props.register(`goals.${props.index}.dimension2`)}
        />     
      }

      {(selectedGoal?.dimension3?.allowedOptions as any[])?.length > 1 ? <>
        <h4 className="dimension-3" >{window.themeConfig.text.crowdfunding.goalSelectDimension3}</h4>
        <div className="select__outer dimension-3"  id="dimension-3">
          <div className="select">
              <div className="select__selected">{selectedGoal?.dimension3?.allowedOptions?.find(d => d.id === goal?.dimension3)?.name || window.themeConfig.text.crowdfunding.goalSelectDimension}</div>

              <input type="hidden" {...props.register(`goals.${props.index}.dimension3`)}/>

              <ul className="select__options">
                  <div className="select__search">

                      <svg>
                          <use xlinkHref="#searchIcon"></use>
                      </svg>

                      <input type="text" id="goals-dimension-3" placeholder={window.themeConfig.text.crowdfunding.searchHerePlaceholder} onKeyUp={() => n3o_cdf_filterList('goals-dimension-3', 'goal-selection-dimension-3')}/>
                  </div>

                  <div className="select__options-body" id="goal-selection-dimension-3">
                    {selectedGoal?.dimension3?.allowedOptions?.map((dimension, dIndex) => (
                      <li
                        key={dIndex}
                        value={dimension.id}
                        onClick={() => {
                          props.setValue(`goals.${props.index}.dimension3`, dimension.id);
                        }}
                      >
                        {dimension.name}
                      </li>
                    ))}
                  </div>
              </ul>

              <svg>
                  <use xlinkHref="#chevron-bottom"></use>
              </svg>
          </div>
        </div>
      </> 
      : <input type="hidden"  {...props.register(`goals.${props.index}.dimension3`)} 
        />     
      }
      
      {(selectedGoal?.dimension4?.allowedOptions as any[])?.length > 1 ? <>
        <h4 className="dimension-4" >{window.themeConfig.text.crowdfunding.goalSelectDimension4}</h4>
        <div className="select__outer dimension-4"  id="dimension-4">
          <div className="select">
              <div className="select__selected">{selectedGoal?.dimension4?.allowedOptions?.find(d => d.id === goal?.dimension4)?.name || window.themeConfig.text.crowdfunding.goalSelectDimension}</div>

              <input type="hidden" {...props.register(`goals.${props.index}.dimension4`)}/>

              <ul className="select__options">
                  <div className="select__search">

                      <svg>
                          <use xlinkHref="#searchIcon"></use>
                      </svg>

                      <input type="text" id="goals-dimension-4" placeholder={window.themeConfig.text.crowdfunding.searchHerePlaceholder} onKeyUp={() => n3o_cdf_filterList('goals-dimension-4', 'goal-selection-dimension-4')}/>
                  </div>

                  <div className="select__options-body" id="goal-selection-dimension-4">
                    {selectedGoal?.dimension4?.allowedOptions?.map((dimension, dIndex) => (
                      <li
                        key={dIndex}
                        value={dimension.id}
                        onClick={() => {
                          props.setValue(`goals.${props.index}.dimension4`, dimension.id);
                        }}
                      >
                        {dimension.name}
                      </li>
                    ))}
                  </div>
              </ul>

              <svg>
                  <use xlinkHref="#chevron-bottom"></use>
              </svg>
          </div>
        </div>
      </> 
      : <input type="hidden" 
        {...props.register(`goals.${props.index}.dimension4`, {value: selectedGoal?.dimension4?.allowedOptions?.[0]?.id})}
        />     
      }
      {selectedGoal?.id ? 
        <>
          <h4>{window.themeConfig.text.crowdfunding.raiseGoalAmount}</h4>
          <div className="cta__input active">
            <label className="large" htmlFor="openAmount">
              {props.data?.currency?.symbol}
            </label>
            <input
              id="openAmount"
              placeholder="75"
              type="text"
              data-type="currency"
              {...props.register(`goals.${props.index}.amount`, {
                required: window.themeConfig.text.crowdfunding.goalAmountRequired,
              })}
            />
            <span> {pricing?.currencyValues?.[goal.currency]?.text} </span>
            <div className="cta__input-select">
              <select
                name="currency"
                {...props.register(`goals.${props.index}.currency`, {
                  required: true,
                  value: props.data?.currency?.id,
                  valueAsNumber: true
                })}
              >
                <option value={props.data?.currency?.id}>{props.data?.currency?.name}</option>
                
              </select>
            </div>
          </div>
        </>
      
      : null}
      
      {goalErrors?.amount && <FieldError message={goalErrors?.amount?.message}/>}
      {selectedGoal?.feedback?.customFields?.map((f, i) =>{
        return <GoalAllocationFields key={f.alias} field={f} fieldIndex={i} goalIndex={props.index} register={props.register} control={props.control}/>
      })}
    </div>
  );
}

