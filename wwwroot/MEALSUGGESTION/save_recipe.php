<?php
// save_recipe.php
$servername = "localhost";
$username = "root";
$password = "";
$database = "mealcraft"; 

// Create connection
$conn = new mysqli($servername, $username, $password, $database);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}


if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $user_id = $_POST['user_id'];
    $recipe_label = $_POST['recipe_label'];
    $recipe_data = $_POST['recipe_data'];
    $recipe_image = $_POST['recipe_image']; // Add this line to get the image URL

    $query = "INSERT INTO liked_recipes (user_id, recipe_label, recipe_data, recipe_image) VALUES (?, ?, ?, ?)";

    if ($stmt = $conn->prepare($query)) {
        $stmt->bind_param("isss", $user_id, $recipe_label, $recipe_data, $recipe_image);
        if ($stmt->execute()) {
            echo json_encode(['success' => true]);
        } else {
            echo json_encode(['success' => false, 'message' => 'Failed to save recipe.']);
        }
        $stmt->close();
    } else {
        echo json_encode(['success' => false, 'message' => 'Failed to prepare statement.']);
    }
    $conn->close();
}
?>
