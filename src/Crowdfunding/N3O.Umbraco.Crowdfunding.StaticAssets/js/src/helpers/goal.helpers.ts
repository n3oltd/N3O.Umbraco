import { FundraiserGoalsRes } from "@n3oltd/umbraco-crowdfunding-client";
import { GoalPricingRef, GoalSchema } from "@/editors/types/goalForm";

export const selectFunc = () => {
  const selects: NodeListOf<Element> = document.querySelectorAll(".select");

  if (!selects) return;

  selects.forEach((select) => {
    const selected = select.querySelector(".select__selected");
    const selectOptions = select?.querySelector(".select__options");
    const listItems = selectOptions?.querySelectorAll("li");
    const input = select?.querySelector("input[type='hidden']");

    const inputElm = input as HTMLInputElement;

    if (!selected) {
      return;
    }

    if (!input || !inputElm) {
      return
    }

    (selected as HTMLElement).onclick = () => {
      select?.classList.toggle("active");
      eventHandler();
    };

    listItems?.forEach((listItem) => {
      listItem.onclick = () => {
        selected.innerHTML = listItem.innerHTML;
        select?.classList.remove("active");
        inputElm.value = listItem.getAttribute("data-value") as any;
        eventHandler();
      };
    });

    const eventHandler = () => {
      window.addEventListener("click", e => {
        if (!select.contains(e.target as Element)) {
          select.classList.remove("active");
        }
      });
    };
  });

  
};

export function n3o_cdf_filterList(inputId: string, listId: string) {
  const input: HTMLElement | null = document.getElementById(inputId);
  const filter = (input as HTMLInputElement)?.value?.toUpperCase();
  const ul: HTMLElement | null = document.getElementById(listId);
  const li = ul?.getElementsByTagName("li");

  if (!li) {
    return;
  }

  // Loop through all list items, and hide those who don't match the search query
  for (let i = 0; i < li.length; i++) {
    if (li[i].innerHTML.toUpperCase().indexOf(filter) > -1) {
      li[i].style.display = "";
    } else {
      li[i].style.display = "none";
    }
  }
}


export function getMinimunAmount(goals) {
  if (!goals) {
    return {};
  }

  const { currency, minimumValues } = goals as any;
  const minimumAmount = minimumValues?.[currency.id];
  return minimumAmount;
}

export function applyPricingRule(pricingRef: React.MutableRefObject<GoalPricingRef | undefined>, g: GoalSchema, goals: FundraiserGoalsRes | undefined): string[] {
  const pricingErrors: string[] = []
  const pricing = pricingRef.current?.[g.goalId as any];

  if (pricing && pricing.locked) {
    return [];
  }

  if (!pricing?.currencyValues) {
    return [];
  }

  const pricingCurrency = pricing.currencyValues?.[g.currency as any];

  if (!pricingCurrency) {
    return [];
  }


    const hasValidAmount = ((g.amount as number) % (pricingCurrency.amount || Number.MAX_VALUE)) === 0;

    if (!hasValidAmount) {
      pricingErrors.push(`${goals?.goalOptions?.find(op => op.id === g.goalId)?.name} must have amount multiple of ${pricingCurrency.text}`);
    }

    return pricingErrors;
}