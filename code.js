var canvas;
var ctx;
var curFaces;

async function setupCamera() {
  video = document.getElementById("video");

  const stream = await navigator.mediaDevices.getUserMedia({
    audio: false,

    video: {
      //for chosing camera
      facingMode: "user",
      //5:4
      aspectRatio: 1.25,

      width: { ideal: 1080 },
    },
  });

  video.srcObject = stream;

  return new Promise((resolve) => {
    video.onloadedmetadata = () => {
      resolve(video);
    };
  });
}

async function renderPrediction() {
  const facepred = await fmesh.estimateFaces(video);

  ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

  if (facepred.length > 0) {
    curFaces = facepred;

    for (face of curFaces) {
      drawFace(face);
    }
  }

  requestAnimationFrame(renderPrediction);
}
async function drawFace(face) {
  drawLips(face);
}

function drawLips(face) {
  const lipLeft = Math.min(
    ...face.annotations.lipsUpperOuter.map((elem) => elem[0])
  );

  const lipRight = Math.max(
    ...face.annotations.lipsUpperOuter.map((elem) => elem[0])
  );

  const lipTop = Math.min(
    ...face.annotations.lipsUpperOuter.map((elem) => elem[1])
  );

  const lipBottom = Math.max(
    ...face.annotations.lipsLowerOuter.map((elem) => elem[1])
  );

  const lipWidth = lipRight - lipLeft;

  const lipHeight = lipBottom - lipTop;

  const customLipImage = new Image();

  customLipImage.src = "lip.png";

  customLipImage.onload = function () {
    ctx.drawImage(
      customLipImage,
      lipLeft - lipWidth * 0.05,
      lipTop - lipHeight * 0.5,
      lipWidth + lipWidth * 0.15,
      lipHeight + lipHeight * 1.2
    );
  };
}

async function main() {
  fmesh = await facemesh.load({ maxFaces: 3 });

  await setupCamera();

  videoWidth = video.videoWidth;

  videoHeight = video.videoHeight;

  video.play();

  canvas = document.getElementById("facecanvas");

  canvas.width = videoWidth;

  canvas.height = videoHeight;

  ctx = canvas.getContext("2d");

  renderPrediction();
}
