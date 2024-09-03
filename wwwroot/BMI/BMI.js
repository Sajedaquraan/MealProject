// Target Elements
const bmiForm = document.querySelector(".users-info");
const usersBMIScoreEl = document.querySelector("#bmi-score");
const usersBMISummary = document.querySelector("#bmi-summary");
const greetingEl = document.getElementById("greeting");
const resultSummaryContainerEl = document.querySelector(
  ".result-summary__container"
);

// Input Field Target Elements
const fieldsetDetailsEl = document.querySelector(".fieldset-details");
const fieldsetUnitEl = document.querySelector(".fieldset-unit");
const numberInputEl = document.querySelectorAll("input[type='number']");

// Configuration
const MIN_BMI_IDEAL = 18.5;
const MAX_BMI_IDEAL = 24.9;
const OVER_WEIGHT_RATE = 29.9;
const OBESE_RATE = 34.9;

const BMI_IMPERIAL_MULTIPLIER = 703;
const CM_TO_M = 100;
const FEET_TO_INCH = 12;
const ST_TO_LBS = 14;

// Helper Function
const cmToMeter = num => num / CM_TO_M;
const feetToInch = num => num * FEET_TO_INCH;
const stToLbs = num => num * ST_TO_LBS;

// BMI Calculator to process User's Data
const bmiCalculator = {
  isMetric: true,

  calculateMetricBMI() {
    return (
      usersInfo.weightInKG / Math.pow(cmToMeter(usersInfo.heightInCM), 2)
    ).toFixed(1);
  },

  calculateImperialBMI() {
    const totalInches =
      feetToInch(usersInfo.heightInFeet) + usersInfo.heightInInch;

    const totalLbs = stToLbs(usersInfo.weightInSt) + usersInfo.weightInLbs;

    return (
      (totalLbs / Math.pow(totalInches, 2)) *
      BMI_IMPERIAL_MULTIPLIER
    ).toFixed(1);
  },

  idealWeightinKG() {
    const minWeight = (
      Math.pow(cmToMeter(usersInfo.heightInCM), 2) * MIN_BMI_IDEAL
    ).toFixed(1);

    const maxWeight = (
      Math.pow(cmToMeter(usersInfo.heightInCM), 2) * MAX_BMI_IDEAL
    ).toFixed(1);

    return [minWeight, maxWeight];
  },

  idealWeightImperial() {
    const totalMinWeight =
      (Math.pow(
        feetToInch(usersInfo.heightInFeet) + usersInfo.heightInInch,
        2
      ) *
        MIN_BMI_IDEAL) /
      BMI_IMPERIAL_MULTIPLIER;

    const totalMaxWeight =
      (Math.pow(
        feetToInch(usersInfo.heightInFeet) + usersInfo.heightInInch,
        2
      ) *
        MAX_BMI_IDEAL) /
      BMI_IMPERIAL_MULTIPLIER;

    const minWeightSt = Math.floor(totalMinWeight / ST_TO_LBS);
    const minWeightLbs = Math.floor(totalMinWeight % ST_TO_LBS);
    const maxWeightSt = Math.floor(totalMaxWeight / ST_TO_LBS);
    const maxWeightLbs = Math.floor(totalMaxWeight % ST_TO_LBS);

    return [minWeightSt, minWeightLbs, maxWeightSt, maxWeightLbs];
  },

  resetInput() {
    usersInfo = {
      ...((bmiCalculator.isMetric && { heightInCM: 0, weightInKG: 0 }) || {
        heightInFeet: 0,
        heightInInch: 0,
        weightInLbs: 0,
        weightInSt: 0,
      }),
    };

    numberInputEl.forEach(input => (input.value = ""));

    greetingEl.innerHTML = "Welcome!";

    resultSummaryContainerEl.classList.remove("activated");
  },

  changeDisplay(e) {
    const lastClass = bmiForm.classList[1];
    const chosenRadio = e.target.id;
    bmiForm.classList.remove(lastClass);
    bmiForm.classList.add(chosenRadio);
  },

  showBMISummary() {
    resultSummaryContainerEl.classList.add("activated");
    greetingEl.innerHTML = "Your BMI is ...";
    const bmiScore = this.isMetric
      ? this.calculateMetricBMI()
      : this.calculateImperialBMI();
    usersBMIScoreEl.innerHTML = bmiScore;
    usersBMISummary.innerHTML = this.bmiDescription(Number(bmiScore));
  },

  bmiDescription(bmi) {
    let bmiStatus;

    if (bmi < MIN_BMI_IDEAL) bmiStatus = "under weight";
    else if (bmi <= MAX_BMI_IDEAL) bmiStatus = "healthy weight";
    else if (bmi <= OVER_WEIGHT_RATE) bmiStatus = "over weight";
    else if (bmi <= OBESE_RATE) bmiStatus = "obese";
    else bmiStatus = "extremely obese";

    if (this.isMetric)
      return `Your BMI suggests you're a <strong>${bmiStatus}</strong>. Your ideal weight is between <strong>${
        this.idealWeightinKG()[0]
      }kgs - ${this.idealWeightinKG()[1]}kgs.</strong>`;
    else
      return `Your BMI suggests you're a <strong>${bmiStatus}</strong>. Your ideal weight is between <strong>${
        this.idealWeightImperial()[0]
      }st ${this.idealWeightImperial()[1]}lbs - ${
        this.idealWeightImperial()[2]
      }st ${this.idealWeightImperial()[3]}lbs.</strong>`;
  },
};

// User Data to be processed
let usersInfo = {
  ...((bmiCalculator.isMetric && { heightInCM: 0, weightInKG: 0 }) || {
    heightInFeet: 0,
    heightInInch: 0,
    weightInLbs: 0,
    weightInSt: 0,
  }),
};

// Event Delegations
fieldsetDetailsEl.addEventListener("focusout", function (e) {
  const entry = e.target;

  // Manipulate User's Data
  usersInfo[entry.id] = entry.valueAsNumber;

  if (
    (usersInfo.heightInCM && usersInfo.weightInKG) ||
    (usersInfo.heightInInch && usersInfo.weightInLbs)
  )
    bmiCalculator.showBMISummary();
});

fieldsetUnitEl.addEventListener("change", function (e) {
  bmiCalculator.changeDisplay(e);

  bmiCalculator.isMetric = !bmiCalculator.isMetric;

  bmiCalculator.resetInput();
});
