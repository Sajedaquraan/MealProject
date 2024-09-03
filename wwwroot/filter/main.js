changeScreen();

document.querySelector("#submit").addEventListener('click', sendApiRequest);
document.querySelector("#submit-right").addEventListener('click', sendApiRequest);

async function sendApiRequest() {
    const APP_ID = "7aa516a5";
    const APP_KEY = "dc836a223fb788b11ae390504d9e97ce";

    let searchvalue = document.querySelector("#search").value.trim();
    let calvalue = document.querySelector("#kcal-select").value;
    let customizations = "";

    let searchBar = document.querySelector("#search");
    let calNumBar = document.querySelector("#kcal-select");

    if (searchvalue == "") {
        searchBar.style.borderColor = 'rgb(239, 61, 62)';
        calNumBar.style.borderColor = "rgb(140, 140, 140)";
        document.querySelector("#search-key-btn").click();
    } else if (document.querySelector(".check_box_cal").checked && calvalue == "") {
        calNumBar.style.borderColor = 'rgb(239, 61, 62)';
        searchBar.style.borderColor = "rgb(140, 140, 140)";
        document.querySelector("#cal-btn").click();
    } else {
        searchBar.style.borderColor = "rgb(140, 140, 140)";
        calNumBar.style.borderColor = "rgb(140, 140, 140)";

        customizations += "q=" + searchvalue + "&app_id=" + APP_ID + "&app_key=" + APP_KEY;

        let selections = document.querySelectorAll(".check_box");
        selections.forEach(selection => {
            if (selection.checked) {
                customizations += "&" + selection.name;
            }
        });

        if (document.querySelector(".check_box_cal").checked) {
            customizations += "&calories=0-" + document.querySelector("#kcal-select").value;
        }

        // Add pagination parameters to retrieve more results
        customizations += "&from=0&to=100"; // Adjust 'to' value as needed

        document.querySelector("#load-screen").click();
        changeScreen();

        let response = await fetch(`https://api.edamam.com/search?${customizations}`);
        let data = await response.json();
        let hits = data.hits;

        if (hits.length === 0) {
            alert("Oops, nothing in our recipes database matches what you are searching for! Please try again.");

            document.querySelector("#search-key-btn").click();
            changeScreen();
        } else {
            console.log(hits);
            document.querySelector("#rec-screen").click();
            changeScreen();

            let html = `<h1>Recipes for <span>${searchvalue}<span><h1>`;

            hits.forEach(hit => {
                let calNum = Math.round(hit.recipe.calories);
                let yield = hit.recipe.yield;
                let healthLabelsUnorg = hit.recipe.healthLabels;
                let healthInfo = healthLabelsUnorg[0];

                let protVal = Math.round(hit.recipe.totalNutrients.PROCNT.quantity);
                let fatVal = Math.round(hit.recipe.totalNutrients.FAT.quantity);
                let carbVal = Math.round(hit.recipe.totalNutrients.CHOCDF.quantity);

                let cholVal = Math.round(hit.recipe.totalNutrients.CHOLE.quantity);
                let sodVal = Math.round(hit.recipe.totalNutrients.NA.quantity);
                let calciVal = Math.round(hit.recipe.totalNutrients.CA.quantity);
                let magVal = Math.round(hit.recipe.totalNutrients.MG.quantity);
                let potVal = Math.round(hit.recipe.totalNutrients.K.quantity);
                let ironVal = Math.round(hit.recipe.totalNutrients.FE.quantity);

                healthLabelsUnorg.shift();
                healthLabelsUnorg.forEach(healthLabel => {
                    healthInfo += " • " + healthLabel;
                });

                html += `
                <div id="recipe-card">
                        <div id="top-row">
                            <img src="${hit.recipe.image}" id="recipe-img">
                            <div>
                                <a href="${hit.recipe.url}">${hit.recipe.label}</a>
                                <br>
                                <br>
                                <h2 id="description">${healthInfo}</h2>
                            </div>
                        </div>

                        <div id="bottom-row">
                            <div id="left-rec-col">
                                <h2>${yield} serving(s)</h2>
                                <br>
                                <h6>${calNum}<span> kcal</span></h6>
                            </div>

                            <div id="stat-seperator">
                                <ul id="health-types">
                                    <li>PROTEIN</li>
                                    <li>FAT</li>
                                    <li>CARB</li>
                                </ul>

                                <ul id="num-val">
                                    <li>${protVal} g</li>
                                    <li>${fatVal} g</li>
                                    <li>${carbVal} g</li>
                                </ul>
                            </div>

                            <div id="stat-seperator">
                                <ul id="health-types">
                                    <li>Cholesterol</li>
                                    <li>Sodium</li>
                                    <li>Calcium</li>
                                    <li>Magnesium</li>
                                    <li>Potassium</li>
                                    <li>Iron</li>
                                </ul>
                                
                                <ul id="num-val">
                                    <li>${cholVal} mg</li>
                                    <li>${sodVal} mg</li>
                                    <li>${calciVal} mg</li>
                                    <li>${magVal} mg</li>
                                    <li>${potVal} mg</li>
                                    <li>${ironVal} mg</li>
                                </ul>
                            </div>
                        </div>
                    </div>`;
            });

            document.querySelector('#rec-col').innerHTML = html;
        }
    }
}

// ...

    function changeScreen() {
        let screens = document.querySelectorAll(".screen");
        screens.forEach(screen => {
            screen.style.display = 'none';
        });

        let checks = document.querySelectorAll(".check");
        checks.forEach(check => {
            if (check.checked) {    
                switch (check.id) {
                    case "search-key-btn":
                        document.querySelector("#search-key-col").style.display = 'block';
                        break;
                    case "allergy-btn":
                        document.querySelector("#allergy-col").style.display = 'block';
                        break;
                    case "diet-btn":
                        document.querySelector("#diet-col").style.display = 'block';
                        break;
                    case "cal-btn":
                        document.querySelector("#cal-col").style.display = 'block';
                        break;
                    case "nut-btn":
                        document.querySelector("#nut-col").style.display = 'block';
                        break;
                    case "rec-screen":
                        document.querySelector("#rec-col").style.display = 'block';
                        break;
                    case "load-screen":
                        document.querySelector("#load-col").style.display = 'block';
                        break;
                }
            }
        });
    }

// Set the diet page as active by default
document.addEventListener('DOMContentLoaded', () => {
    document.querySelector("#diet-btn").checked = true;
    changeScreen();
});

document.querySelector("#methods").addEventListener("click", changeScreen);

function addButtonRight() {
    let html = "";

    let selections = document.querySelectorAll(".check_box");
    let calvalue = document.querySelector("#kcal-select").value;

    selections.forEach(selection => {
        if (selection.checked) {
            html += `<div>
                        <input type="checkbox" id="${selection.id}" class="check_box_right" checked="checked">
                        <label for="${selection.id}">${selection.value}</label>
                     </div>`;
        }
    });

    if (document.querySelector(".check_box_cal").checked && calvalue != "") {
        html += `<div>
                    <input type="checkbox" id="kcal-check-id" class="check_box_right" checked="checked">
                    <label for="kcalIdRight">${calvalue} kcal</label>
                </div>`;
    }

    document.querySelector('#clicked-buttons').innerHTML = html;
}

document.querySelector("#second-col").addEventListener("click", addButtonRight);

function removeButtonMiddle() {
    let rightBtns = document.querySelectorAll(".check_box_right");

    rightBtns.forEach(btn => {
        if (!btn.checked) {
            document.getElementById(btn.id).click();
        }
    });

    addButtonRight();
}

document.querySelector("#third-col").addEventListener("click", removeButtonMiddle);
