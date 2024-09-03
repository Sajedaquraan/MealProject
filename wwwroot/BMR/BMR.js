// Target Elements
const bmiForm = document.querySelector(".users-info");
const usersBMIScoreEl = document.querySelector("#bmi-score");
const usersBMISummary = document.querySelector("#bmi-summary");
const greetingEl = document.getElementById("greeting");
const resultSummaryContainerEl = document.querySelector(".result-summary__container");

// Input Field Target Elements
const fieldsetDetailsEl = document.querySelector(".fieldset-details");
const fieldsetUnitEl = document.querySelector(".fieldset-unit");
const numberInputEl = document.querySelectorAll("input[type='number']");

// User Data to be processed
let usersInfo = {
  heightInCM: 0,
  weightInKG: 0,
  age: 0,
  gender: ''
};

// Event Delegations
fieldsetDetailsEl.addEventListener("input", function (e) {
  const entry = e.target;

  // Manipulate User's Data
  usersInfo[entry.id] = entry.valueAsNumber;

  if (usersInfo.heightInCM && usersInfo.weightInKG && usersInfo.age && usersInfo.gender) {
    showBMRSummary();
  }
});

document.querySelectorAll("input[name='gender']").forEach(genderInput => {
  genderInput.addEventListener("change", function (e) {
    usersInfo.gender = e.target.value;
    if (usersInfo.heightInCM && usersInfo.weightInKG && usersInfo.age && usersInfo.gender) {
      showBMRSummary();
    }
  });
});

// BMR Calculator to process User's Data
function calculateBMR() {
  const { weightInKG: W, heightInCM: H, age: A, gender: G } = usersInfo;
  if (G === 'male') {
    return 10 * W + 6.25 * H - 5 * A + 5;
  } else if (G === 'female') {
    return 10 * W + 6.25 * H - 5 * A - 161;
  }
  return 0;
}

function showBMRSummary() {
  resultSummaryContainerEl.classList.add("activated");
  greetingEl.innerHTML = "Your BMR is ...";
  const bmrScore = calculateBMR();
  usersBMIScoreEl.innerHTML = bmrScore.toFixed(2);
  usersBMISummary.innerHTML = `Based on the information provided, your Basal Metabolic Rate (BMR) is <strong>${bmrScore.toFixed(2)}</strong> calories/day.`;
}

fieldsetUnitEl.addEventListener("change", function (e) {
  const chosenRadio = e.target.id;
  usersInfo = {
    heightInCM: 0,
    weightInKG: 0,
    age: 0,
    gender: ''
  };
  numberInputEl.forEach(input => (input.value = ""));
  greetingEl.innerHTML = "Welcome!";
  resultSummaryContainerEl.classList.remove("activated");
});
