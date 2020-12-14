const { log } = require("console");

fs = require("fs");

input = fs.readFileSync("./input.txt").toString();

lines = input.split("\n");

let n = lines[0].split(" ").map(Number);

function read_first_layer_from_input() {
  const layer1 = [];
  for (let i = 1; i < lines.length; i++) {
    layer1.push(lines[i].split(" ").map(Number));
  }
  return layer1;
}
function initialize_layer2(layer1row, layer1Column) {
  const layer2 = [];
  for (let i = 0; i < layer1row; i++) {
    layer2.push(new Array(layer1Column).fill(0));
  }
  return layer2;
}
function canPutBrick(layer1, layer2, row, column, isHorizontal) {
  if (isHorizontal && layer1[row][column] !== layer1[row][column + 1]) {
    return true;
  } else if (!isHorizontal && layer1[row][column] === layer1[row][column + 1]) {
    return true;
  }
  return false;
}

function putBrick(layer2, row, column, isHorizontal, newBrick) {
  if (isHorizontal && layer2[row][column + 1] !== undefined) {
    layer2[row][column] = newBrick;
    layer2[row][column + 1] = newBrick;
  }

  if (!isHorizontal || layer2[row][column + 1] === undefined) {
    layer2[row][column] = newBrick;
    layer2[row + 1][column] = newBrick;
  }
}

function getSecondLayer(layer1) {
  const layer2 = initialize_layer2(layer1.length, layer1[0].length);
  let newBrick = 1;

  for (let i = 0; i < layer2.length; i++) {
    for (let j = 0; j < layer2[i].length; j++) {
      if (layer2[i][j] !== 0) {
        continue;
      }
      if (canPutBrick(layer1, layer2, i, j, true)) {
        putBrick(layer2, i, j, true, newBrick);
        newBrick += 1;
      } else if (canPutBrick(layer1, layer2, i, j, false)) {
        putBrick(layer2, i, j, false, newBrick);
        newBrick += 1;
      } else {
        return null;
      }
    }
  }
  return layer2;
}
function print_layer(layer2) {
  for (let i = 0; i < layer2.length; i++) {
    let prevBrick = layer2[i][0];
    let line = "";
    for (let j = 0; j < layer2[i].length; j++) {
      if (prevBrick !== layer2[i][j]) {
        line += "*" + layer2[i][j].toString();
        prevBrick = layer2[i][j];
      } else {
        line += layer2[i][j].toString();
        prevBrick = layer2[i][j];
      }
    }
    console.log(line);
  }
}

function validationInput() {
  if (n[0] % 2 === 0 && n[1] % 2 === 0 && n[0] < 100 && n[1] < 100) {
    return true;
  }
  return false;
}
function problem_solution() {
  if (validationInput()) {
    const layer1 = read_first_layer_from_input();
    const layer2 = getSecondLayer(layer1);
    print_layer(layer2);
  } else {
    console.log(-1);
  }
}
problem_solution();