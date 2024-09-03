<?php
session_start();

$response = array('logged_in' => false);

if (isset($_SESSION['userid'])) {
    $response['logged_in'] = true;
    $response['user_id'] = $_SESSION['userid'];
}

echo json_encode($response);
?>
