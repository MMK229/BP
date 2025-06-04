<?php
include("RtcTokenBuilder2.php");
// Get the value of the environment variable AGORA_APP_ID. Make sure you set this variable to the App ID you obtained from Agora console.
$appId = "42bef5727d2a468190b11cc1d533a7a7";
// Get the value of the environment variable AGORA_APP_CERTIFICATE. Make sure you set this variable to the App certificate you obtained from Agora console
$appCertificate = "c07caad00e20473589f9414f1f644a2b";
// Replace channelName with the name of the channel you want to join
$channelName = "7d72365eb983485397e3e3f9d460bdda";
// Fill in your actual user ID
$uid = 0;
// Token validity time in seconds
$tokenExpirationInSeconds = 3600;
// The validity time of all permissions in seconds
$privilegeExpirationInSeconds = 3600;

header("Content-Type: application/json");
header("Access-Control-Allow-Origin: *");
// Generate Token
$token = RtcTokenBuilder2::buildTokenWithUid($appId, $appCertificate, $channelName, $uid, RtcTokenBuilder2::ROLE_PUBLISHER, $tokenExpirationInSeconds, $privilegeExpirationInSeconds);
echo "{\"token\":\"$token\"}";
