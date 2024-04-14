<?php
$con = mysqli_connect('localhost', 'root', 'root', 'majorproject');
// Check if the connection happened
if(mysqli_connect_errno()) {
    echo "1: Connection Failed"; // Error code #1 = connection failed
    exit();
}

// Get username and password from POST request
$username = mysqli_real_escape_string($con, $_POST["name"]);
$password = mysqli_real_escape_string($con, $_POST["password"]);
$email = mysqli_real_escape_string($con, $_POST["email"]);

// Check if username already exists
$namecheckquery = "SELECT username FROM user WHERE username = '$username';";
$namecheck = mysqli_query($con, $namecheckquery);
if(!$namecheck) {
    echo "2: Name Check Query Failed: " . mysqli_error($con);
    exit();
}
if(mysqli_num_rows($namecheck) > 0) {
    echo "3: Name Already Exists"; // Error code #3 = name exists cannot register
    exit();
}

// Add user to the table
$salt = "\$5\$rounds=1000\$" . "hoohahhoo" . $username . "\$";
$hash = crypt($password, $salt);
$insertuserquery = "INSERT INTO user (username, hash, salt, email) VALUES ('$username', '$hash', '$salt', '$email');";
$insertuser = mysqli_query($con, $insertuserquery);
if(!$insertuser) {
    echo "4: Insert UserQuery Failed: " . mysqli_error($con);
    exit();
}

echo "0"; // Success
?>
