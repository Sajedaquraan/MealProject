
  
  function getParameterByName(name) {
            const url = window.location.href;
            const nameRegex = name.replace(/[\[\]]/g, '\\$&');
            const regex = new RegExp('[?&]' + nameRegex + '(=([^&#]*)|&|#|$)');
            const results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        }

        function simulateClick(button) {
            button.click();
        }

        document.addEventListener('DOMContentLoaded', function () {
            const ingredients = getParameterByName('ingredients');
            if (ingredients) {
                const searchBar = document.querySelector('input[name="search-bar"]');
                searchBar.value = ingredients;

                const searchButton = document.getElementById('search');
                if (searchButton) {
                    simulateClick(searchButton);
                }
            }
        });

        





  function getParameterByName(name) {
      const url = window.location.href;
      const nameRegex = name.replace(/[\[\]]/g, '\\$&');
      const regex = new RegExp('[?&]' + nameRegex + '(=([^&#]*)|&|#|$)');
      const results = regex.exec(url);
      if (!results) return null;
      if (!results[2]) return '';
      return decodeURIComponent(results[2].replace(/\+/g, ' '));
  }

  function simulateClick(button) {
      button.click();
  }

  document.addEventListener('DOMContentLoaded', function () {
      const ingredients = getParameterByName('ingredients');
      if (ingredients) {
          const searchBar = document.querySelector('input[name="search-bar"]');
          searchBar.value = ingredients;

          const searchButton = document.getElementById('search');
          if (searchButton) {
              simulateClick(searchButton);
          }
      }
  });

  //
// ----------- Main Section - Search bar
//

// Selectors
const searchButton = document.querySelector("#search");
const recipeCards = document.querySelector("#recipe-cards__main");
const errorHandling = document.querySelector("#main-container__error-handling");
const clearIcon = document.querySelector("#recipe-form__clear-icon");
const searchBar = document.querySelector("#recipe-form__search-bar");
const showMoreButton = document.querySelector("#show-more");
const searchForm = document.querySelector("form");
const searchResultDiv = document.querySelector(".search-result");
const container = document.querySelector(".container");
const maxButtonsToShow = 5;
let searchQuery = "";
const APP_ID = "ba5b7a21";
const APP_key = "d1d3afcdc37dd030c29294267aaedbc8";
let currentPage = 1; // Track the current page
const resultsPerPage = 10; // Number of results to display per page

// Event listeners
searchBar.addEventListener('keydown', e => {
if (e.keyCode === 13) {
  // preventDefault to stop reloading page when event is fired
  e.preventDefault();
  searchQuery = searchBar.value;
  currentPage = 1; // Reset to the first page when a new search is performed
  fetchAPI();
}
});

searchButton.addEventListener("click", () => {
searchQuery = searchBar.value;
currentPage = 1; // Reset to the first page when a new search is performed
fetchAPI();
});

// Event listeners for the clear icon to be visible in the search bar
searchBar.addEventListener("keyup", () => {
if (searchBar.value && clearIcon.style.visibility != "visible") {
  clearIcon.style.visibility = "visible";
} else if (!searchBar.value) {
  clearIcon.style.visibility = "hidden";
}
});

clearIcon.addEventListener("click", () => {
searchBar.value = "";
clearIcon.style.visibility = "hidden";
});

showMoreButton.addEventListener("click", () => {
    currentPage++;
    fetchAPI();
});

// Get value from ticked checkbox
function checkboxDietLabel() {
let checkboxDiet = document.forms[0];
let i;
let checkboxDietValueArray = [];

for (i = 0; i < checkboxDiet.length; i++) {
  if (checkboxDiet[i].checked) {
      checkboxDietValueArray.push(checkboxDiet[i].value);
  }
}

if (checkboxDietValueArray.length === 0) {
  return "";
} else if (checkboxDietValueArray.length === 1) {
  return `&diet=${checkboxDietValueArray[0]}`;
} else {
  let checkboxDietValue = "";
  for (let j = 0; j < checkboxDietValueArray.length; j++) {
      checkboxDietValue += `&diet=${checkboxDietValueArray[j]}`;
  }
  return checkboxDietValue;
}
}
async function fetchAPI() {
    let dietLabels = checkboxDietLabel();
    const from = (currentPage - 1) * resultsPerPage;
    const to = currentPage * resultsPerPage;
    const baseURL = `https://api.edamam.com/search?app_id=${APP_ID}&app_key=${APP_key}&q=${searchQuery}${dietLabels}&from=${from}&to=${to}`;
    try {
        const response = await fetch(baseURL);
        const data = await response.json();
        console.log(data);
        generateHTML(data.hits);

        // Show the "Show More" button if there are more results to load
        if (data.more) {
            showMoreButton.style.display = "block";
        } else {
            showMoreButton.style.display = "none";
        }
    } catch (err) {
        recipeCards.innerHTML = "";
        errorHandling.innerHTML = `
        <div class="container error-handling">
            <div class="row">
                <div class="col">
                    <i class="fas fa-exclamation-triangle"></i>
                    <p>Ops something went wrong, please change the search term or the filters and try again.</p>
                </div>
            </div>
        </div>`;
        console.log(err);
    }
}

let likedRecipes = new Set();


function generateHTML(results) {
    errorHandling.innerHTML = "";
  
    results.forEach((result) => {
        let healthLabels = result.recipe.healthLabels;
        let healthLabelsPillsHTML = "";
        healthLabels.forEach((label) => {
            healthLabelsPillsHTML += `<span class="badge badge-pill badge-success">${label}</span>`;
        });
  
        let heartIconClass = "";
        
        recipeCards.innerHTML += `
  <div class="col-12 col-md-6 col-xl-4 recipe-cards__card-wrapper">
    <div class="card recipe-cards__card" style="width: 28rem;height:auto">
        <img src="${result.recipe.image}" class="card-img-top recipe-cards__img" alt="Meal image">
        <span onclick="toggleHeartIcon(this, '${result.recipe.label}', '${encodeURIComponent(JSON.stringify(result.recipe.url))}', '${result.recipe.image}')"
                    class="heart-icon recipe-cards__heart ${heartIconClass}">
                    <i style=font-size:2.2rem class="heart far fa-heart heart"></i>
                </span>
        <div class="card-body">
            <h3 style=margin-top:20px;font-size:28px class="card-title recipe-cards__title">${result.recipe.label}</h3>
            <p class="recipe-cards__text">Cuisine Type: ${result.recipe.cuisineType}</p>
            <p class="recipe-cards__text">Total Calories: ${result.recipe.calories.toFixed(0)}</p>
            <div style=margin-right: 5px;  class="recipe-cards__badges list-group-item li-health-labels">${healthLabelsPillsHTML}</div>
            <div class="d-flex justify-content-between align-items-center">
                <a style=color:black href="${result.recipe.url}" target="_blank" class="btn btn-primary" rel="noopener" tabindex="0">See Recipe</a>
            </div>
        </div>
    </div>
  </div>
  `;
  
    });
  }
  


function displayErrorMessage(message) {
  const errorHandling = document.getElementById("main-container__error-handling");
  errorHandling.innerHTML = `<div class="alert alert-danger" role="alert">${message}</div>`;
}
function toggleHeartIcon(icon, recipeLabel, recipeData, recipeImage) {
 
// Check if user is logged in
fetch('./check_login.php')
  .then(response => response.json())
  .then(data => {
      if (data.logged_in) {
        
          // User is logged in, proceed with saving the recipe
          saveRecipe(icon, recipeLabel, recipeData, recipeImage, data.user_id);
      } else {
          // User is not logged in, redirect to login page
          alert("You must loggin to your profile.");
          window.location.href = '../login/login.html';
      }
      
  })
  .catch(error => {
      console.error('Error:', error);
      displayErrorMessage('An error occurred while checking the login status.');
  });
}


function saveRecipe(icon, recipeLabel, recipeData, recipeImage, userId) {
if (!icon.classList.contains('liked')) {
  icon.classList.add("liked");
  icon.querySelector('.heart').classList.remove("far");
  icon.querySelector('.heart').classList.add("fas");
  likedRecipes.add(recipeLabel);

  // Send to backend
  const data = new URLSearchParams();
  data.append('user_id', userId);
  data.append('recipe_label', recipeLabel);
  data.append('recipe_data', decodeURIComponent(recipeData));
  data.append('recipe_image', recipeImage); // Add the image URL

  fetch('./save_recipe.php', {
      method: 'POST',
      body: data,
      headers: {
          'Content-Type': 'application/x-www-form-urlencoded',
      },
  })
  .then(response => response.json())
  .then(data => {
      if (data.success) {
          alert(`Meal "${recipeLabel}" has been saved to your profile.`);
      } else {
          displayErrorMessage(`Error: ${data.message}`);
      }
  })
  .catch(error => {
      console.error('Error:', error);
      displayErrorMessage('An error occurred while saving the recipe.');
  });
} else {
  displayErrorMessage(`You have already saved "${recipeLabel}" to your profile.`);
}
}





// Function to display error messages
function displayErrorMessage(message) {
errorHandling.innerHTML = `<div class="alert alert-danger">${message}</div>`;
}



function displayErrorMessage(message) {
errorHandling.innerHTML = `
<div class="error-message">
  <p>${message}</p>
</div>`;

setTimeout(() => {
  errorHandling.innerHTML = "";
}, 3000); 
}



function getCardByLabel(label) {
return cardData.find(card => card.label === label);
}









function displayErrorMessage(message) {
errorHandling.innerHTML = `
<div class="error-message">
  <p>${message}</p>
</div>`;

// Clear error message after some time (e.g., 3 seconds)
setTimeout(() => {
  errorHandling.innerHTML = "";
}, 3000); // 3000 milliseconds = 3 seconds
}






// Function to check if an element is in the viewport
function isElementInViewport(element) {
const rect = element.getBoundingClientRect();
return (
  rect.top >= 0 &&
  rect.left >= 0 &&
  rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
  rect.right <= (window.innerWidth || document.documentElement.clientWidth)
);
}

// Function to handle scroll and check if elements are in the viewport
function handleScroll() {
const elements = document.querySelectorAll(".fade-in");

elements.forEach((element) => {
  if (isElementInViewport(element)) {
      element.classList.add("fade-in-active");
  }
});
}

// Add a scroll event listener to trigger the animation
window.addEventListener("scroll", handleScroll);

// Initial check when the page loads
handleScroll();



