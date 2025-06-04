// create Agora client
var client = AgoraRTC.createClient({ mode: "rtc", codec: "vp8" });

var localTracks = {
  videoTrack: null,
  audioTrack: null
};
var remoteUsers = {};
// Agora client options
var options = {
  appid: null,
  channel: null,
  uid: null,
  token: null
};

// Parameters
$(() => {
  var urlParams = new URL(location.href).searchParams;
  options.appid = urlParams.get("appid");
  options.channel = urlParams.get("channel");
  options.token = urlParams.get("token");
  options.uid = urlParams.get("uid");

  if (options.appid && options.channel) {
    $("#appid").val(options.appid);
    $("#channel").val(options.channel);
    $("#token").val(options.token || "");
    if (options.uid) {
      $("#uid").val(options.uid);
    }

    // Populate the token option automatically
    if (!options.token) {
      fetch(`https://tribec.dev/bp/RtcTokenBuilder2Sample.php?channelName=${options.channel}`)
          .then(response => response.json())
          .then(data => {
            if (data.token) {
              options.token = data.token;
              $("#token").val(data.token);
              console.log("Token auto-filled from remote source.");
            } else {
              console.warn("Token not found in response.");
            }
          })
          .catch(err => {
            console.error("Failed to fetch token:", err);
          });
    }
  }
});

//
// CAMERAS
//
//Cam1
$("#join-cam-1").click(async function (e) {
  e.preventDefault();
  $("#join-cam-1").attr("disabled", true);
  $("#join-cam-2").attr("disabled", true);
  $("#join-cam-3").attr("disabled", true);
  $("#join-cam-4").attr("disabled", true);
  $("#join-cam-5").attr("disabled", true);
  try {
    options.appid = $("#appid").val();
    options.token = $("#token").val();
    options.channel = $("#channel").val();
    options.uid = 3395;
    await join();
    if(options.token) {
      $("#success-alert-with-token").css("display", "block");
      $("#camera-status").text("Connected as Camera 1 (ID: 3395)");
      $("#camera-status").css("color", "green");
    } else {
      $("#success-alert a").attr("href", `index.html?appid=${options.appid}&channel=${options.channel}&token=${options.token}&uid=3395`);
      $("#success-alert").css("display", "block");
    }
  } catch (error) {
    console.error(error);
  } finally {
    $("#leave").attr("disabled", false);
  }
});

// Cam2
$("#join-cam-2").click(async function (e) {
  e.preventDefault();
  $("#join-cam-1").attr("disabled", true);
  $("#join-cam-2").attr("disabled", true);
  $("#join-cam-3").attr("disabled", true);
  $("#join-cam-4").attr("disabled", true);
  $("#join-cam-5").attr("disabled", true);
  try {
    options.appid = $("#appid").val();
    options.token = $("#token").val();
    options.channel = $("#channel").val();
    options.uid = 3396;
    await join();
    if(options.token) {
      $("#success-alert-with-token").css("display", "block");
      $("#camera-status").text("Connected as Camera 2 (ID: 3396)");
      $("#camera-status").css("color", "green");
    } else {
      $("#success-alert a").attr("href", `index.html?appid=${options.appid}&channel=${options.channel}&token=${options.token}&uid=3396`);
      $("#success-alert").css("display", "block");
    }
  } catch (error) {
    console.error(error);
  } finally {
    $("#leave").attr("disabled", false);
  }
});

// Cam3
$("#join-cam-3").click(async function (e) {
  e.preventDefault();
  $("#join-cam-1").attr("disabled", true);
  $("#join-cam-2").attr("disabled", true);
  $("#join-cam-3").attr("disabled", true);
  $("#join-cam-4").attr("disabled", true);
  $("#join-cam-5").attr("disabled", true);
  try {
    options.appid = $("#appid").val();
    options.token = $("#token").val();
    options.channel = $("#channel").val();
    options.uid = 3397;
    await join();
    if(options.token) {
      $("#success-alert-with-token").css("display", "block");
      $("#camera-status").text("Connected as Camera 3 (ID: 3397)");
      $("#camera-status").css("color", "orange");
    } else {
      $("#success-alert a").attr("href", `index.html?appid=${options.appid}&channel=${options.channel}&token=${options.token}&uid=3397`);
      $("#success-alert").css("display", "block");
    }
  } catch (error) {
    console.error(error);
  } finally {
    $("#leave").attr("disabled", false);
  }
});

//Cam4
$("#join-cam-4").click(async function (e) {
  e.preventDefault();
  $("#join-cam-1").attr("disabled", true);
  $("#join-cam-2").attr("disabled", true);
  $("#join-cam-3").attr("disabled", true);
  $("#join-cam-4").attr("disabled", true);
  $("#join-cam-5").attr("disabled", true);
  try {
    options.appid = $("#appid").val();
    options.token = $("#token").val();
    options.channel = $("#channel").val();
    options.uid = 3398;
    await join();
    if(options.token) {
      $("#success-alert-with-token").css("display", "block");
      $("#camera-status").text("Connected as Camera 4 (ID: 3398)");
      $("#camera-status").css("color", "orange");
    } else {
      $("#success-alert a").attr("href", `index.html?appid=${options.appid}&channel=${options.channel}&token=${options.token}&uid=3398`);
      $("#success-alert").css("display", "block");
    }
  } catch (error) {
    console.error(error);
  } finally {
    $("#leave").attr("disabled", false);
  }
});

// Cam5
$("#join-cam-5").click(async function (e) {
  e.preventDefault();
  $("#join-cam-1").attr("disabled", true);
  $("#join-cam-2").attr("disabled", true);
  $("#join-cam-3").attr("disabled", true);
  $("#join-cam-4").attr("disabled", true);
  $("#join-cam-5").attr("disabled", true);
  try {
    options.appid = $("#appid").val();
    options.token = $("#token").val();
    options.channel = $("#channel").val();
    options.uid = 3399;
    await join();
    if(options.token) {
      $("#success-alert-with-token").css("display", "block");
      $("#camera-status").text("Connected as Camera 4 (ID: 3399)");
      $("#camera-status").css("color", "orange");
    } else {
      $("#success-alert a").attr("href", `index.html?appid=${options.appid}&channel=${options.channel}&token=${options.token}&uid=3399`);
      $("#success-alert").css("display", "block");
    }
  } catch (error) {
    console.error(error);
  } finally {
    $("#leave").attr("disabled", false);
  }
});

// Leave
$("#leave").click(function (e) {
  leave();
});

// Join button
async function join() {
  // Add event listener to play remote tracks when remote user publishs.
  client.on("user-published", handleUserPublished);
  client.on("user-unpublished", handleUserUnpublished);

  // Join a channel and create local tracks, we can use Promise.all to run them concurrently
  [ _, localTracks.audioTrack, localTracks.videoTrack ] = await Promise.all([
    // Join with UID respective to camera the user picked
    client.join(options.appid, options.channel, options.token || null, options.uid),
    // Agora video/audio tracks....
    AgoraRTC.createMicrophoneAudioTrack(),
    AgoraRTC.createCameraVideoTrack()
  ]);

  // Play local video track
  localTracks.videoTrack.play("local-player");
  $("#local-player-name").text(`localVideo(${options.uid})`);

  // Publish local tracks to channel
  await client.publish(Object.values(localTracks));
  console.log("publish success with UID:", options.uid);
}

// Leave button
async function leave() {
  for (trackName in localTracks) {
    var track = localTracks[trackName];
    if(track) {
      track.stop();
      track.close();
      localTracks[trackName] = undefined;
    }
  }

  // Remove remote users and player views
  remoteUsers = {};
  // Clear all remote players
  const remotePlayers = document.querySelectorAll('.video-container:not(:first-child)');
  remotePlayers.forEach(player => player.remove());

  // Leave the channel
  await client.leave();
  $("#local-player-name").text("");
  $("#join-cam-1").attr("disabled", false);
  $("#join-cam-2").attr("disabled", false);
  $("#join-cam-3").attr("disabled", false);
  $("#join-cam-4").attr("disabled", false);
  $("#join-cam-5").attr("disabled", false);
  $("#leave").attr("disabled", true);
  $("#camera-status").text("");
  console.log("client leaves channel success");
}

async function subscribe(user, mediaType) {
  const uid = user.uid;
  // subscribe to a remote user
  await client.subscribe(user, mediaType);
  console.log("subscribe success");
  if (mediaType === 'video') {
    const playerContainer = $(`
      <div class="video-container">
        <p class="player-name">remoteUser(${uid})</p>
        <div id="player-${uid}" class="player"></div>
      </div>
    `);
    $("#video-grid").append(playerContainer);
    user.videoTrack.play(`player-${uid}`);
  }
  if (mediaType === 'audio') {
    user.audioTrack.play();
  }
}

function handleUserPublished(user, mediaType) {
  const id = user.uid;
  remoteUsers[id] = user;
  subscribe(user, mediaType);
}

function handleUserUnpublished(user) {
  const id = user.uid;
  delete remoteUsers[id];
  $(`.video-container:has(#player-${id})`).remove();
}

// Fetch tokens
async function fetchTokenAndFill() {
  try {
    const response = await fetch("https://tribec.dev/bp/RtcTokenBuilder2Sample.php");
    const data = await response.json();
    if (data.token) {
      $("#token").val(data.token);
      console.log("Token auto-filled from API.");
    } else {
      console.warn("No token received.");
    }
  } catch (error) {
    console.error("Error fetching token:", error);
  }
}

// Call it on DOM ready
$(document).ready(function () {
  fetchTokenAndFill();
});