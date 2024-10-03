import React from "react";

import { useReactive, useRequest } from "ahooks";
import { useFieldArray, useForm } from "react-hook-form";
import { FundraiserGoalsReq } from "@n3oltd/umbraco-crowdfunding-client";

import { GoalFields } from "./goalForm/GoalFields";
import { Modal } from "./common/Modal"

import { usePageData } from "@/hooks/usePageData";
import { _client } from "@/common/cfClient";
import { applyPricingRule, getMinimunAmount, selectFunc } from "@/helpers/goal.helpers";
import { FormSchema, GoalPricingRef, GoalSchema } from "./types/goalForm";
import { EditorProps } from "./types/EditorProps"
import { loadingToast, updatingToast } from "@/helpers/toaster";

export const GoalEditor: React.FC<EditorProps> = ({
    open,
    onClose
}) => {

    const { pageId } = usePageData()
    const state = useReactive<{ errors: string[] }>({
        errors: []
    })
    const { register, handleSubmit, control, setValue, getValues, trigger } = useForm<FormSchema>();

    const { fields, append, remove } = useFieldArray({
        name: "goals",
        control,
    });

    const { loading, runAsync, data: goals } = useRequest((pageId) => _client.getFundraiserGoals(pageId), {
        manual: true
    })

    const { loading: updating, runAsync: updateGoals, error } = useRequest((pageId, gaols) => _client.updateFundraiserGoals(pageId, gaols), {
        manual: true,
        onSuccess: () => {
            onClose();
            window.location.reload();
        }
    })

    const pricingRef = React.useRef<GoalPricingRef>();

    React.useEffect(() => {
        if (pageId) {
            loadingToast(runAsync(pageId).catch(console.error));
        }
    }, [runAsync, pageId]);


    React.useEffect(() => {
        selectFunc();
    }, [fields.length, goals]);

    React.useEffect(() => {
        if (goals?.selectedGoals) {
            const initialValues: GoalSchema[] =
                goals?.selectedGoals.map((goal) => ({
                    goalId: goal.optionId || '',
                    amount: goal.value,
                    dimension1: goal.fundDimensions?.dimension1,
                    dimension2: goal.fundDimensions?.dimension2,
                    dimension3: goal.fundDimensions?.dimension3,
                    dimension4: goal.fundDimensions?.dimension4,
                    currency: goals?.currency?.id || '',
                    allocations: goal?.feedback?.feedback?.reduce((acc: any, curr: any) => {
                        acc[curr.alias] = curr[curr.type];
                        return acc;
                    }, {})
                }))

            setValue("goals", initialValues);
        }
    }, [setValue, goals]);

    const onSubmit = (data: FormSchema) => {
        let pricingErrors: string[] = [];
        const goalsTotalAmount = data.goals.reduce((acc, curr) => {
            acc += Number(curr.amount)

            return acc
        }, 0);

        const minimumAmountValue = getMinimunAmount(goals);

        const hasValidMinimumAmount = goalsTotalAmount >= minimumAmountValue.amount;

        if (!hasValidMinimumAmount) {
            pricingErrors.push(`Goals total amount should be atleast ${minimumAmountValue.text}`)
            state.errors = pricingErrors;
            return
        }

        const payload: FundraiserGoalsReq = {
            items: data.goals.map(g => {

                pricingErrors = [...pricingErrors, ...applyPricingRule(pricingRef, g, goals)];

                return {
                    amount: g.amount,
                    goalOptionId: g.goalId,
                    fundDimensions: {
                        dimension1: g.dimension1,
                        dimension2: g.dimension2,
                        dimension3: g.dimension3,
                        dimension4: g.dimension4,
                    },
                    feedback: {
                        customFields: {
                            entries: Object.entries(g.allocations || {}).map(([key, value]) => {
                                const gaol = goals?.goalOptions?.find(gOp => gOp.id === g.goalId);
                                const type: any = gaol ? gaol.feedback?.customFields?.find(c => c.alias === key)?.type : "";

                                return {
                                    [type]: value,
                                    alias: key
                                }
                            })
                        }
                    }
                }
            })
        }

        if (!pricingErrors.length) {
            updatingToast(updateGoals(pageId, payload))

        }

        state.errors = pricingErrors;
    };

    const onFormSubmit = async () => {
        try {
            const isValid = await trigger();

            if (isValid) {
                await handleSubmit(onSubmit)();
            }
        } catch (error) {
            console.error(error)
        }

    }

    const errors = JSON.parse((error as any)?.response || '{}')?.errors;

    const minimumAmount = getMinimunAmount(goals)?.text;


    return <Modal
        id="goal-edit"
        isOpen={open}
        onOk={onFormSubmit}

        oKButtonProps={{
            disabled: updating || loading
        }}
    >
        <div className="editGoal__wrapper">
            <div className="editGoal" id="editGoal">
                <div className="editGoal__head">
                    <h4>Edit Goal</h4>
                    <div className="detail">
                        You can modify goal or add more projects to your fundraiser page
                        here.
                    </div>
                </div>
                <div className="editGoal__body-wrapper">
                    <form onSubmit={handleSubmit(onSubmit)}>
                        {fields.map((field, index) => {

                            return (
                                <GoalFields
                                    key={field.id}
                                    control={control}
                                    register={register}
                                    setValue={setValue}
                                    getValues={getValues}
                                    remove={remove}
                                    index={index}
                                    data={goals}
                                    pricingRef={pricingRef}
                                ></GoalFields>
                            );
                        })}

                        {!loading ? <button
                            style={{
                                width: "100%",
                            }}
                            type="button"
                            className="editGoal__add"
                            onClick={() => {
                                append({
                                    goalId: "",
                                    amount: 0,
                                    dimension1: '',
                                    dimension2: '',
                                    dimension3: '',
                                    dimension4: '',
                                    currency: goals?.currency?.id || '',
                                    allocations: {},
                                });
                            }}
                        >
                            <span>
                                <img src="/assets/images/icons/plus-circle.svg" alt="" />
                            </span>
                            <h4>Add Another Project</h4>
                        </button> : null}
                        {((errors && Array.isArray(errors)) || state.errors.length) ? <div className="active checkout__message">
                            <p className="detail">
                                {state.errors.map(e => {
                                    return <li key={e}>{e}</li>
                                })}

                                {errors?.map((e: any) => {
                                    return <li key={e.property}>{e.error}</li>
                                })}

                            </p>
                        </div> : null
                        }
                        <p className="subtle">Note: Minimum amount is {minimumAmount}</p>
                        <div className="editGoal__foot">
                            <button type="button" className="button secondary" onClick={onClose} disabled={updating}>
                                Cancel
                            </button>
                            <button type="submit" className="button primary" disabled={updating}>
                                Save
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </Modal>
}