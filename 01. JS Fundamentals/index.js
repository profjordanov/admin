function sum3Numbers(arr) {
    let num1 = Number(arr[0]);
    let num2 = Number(arr[1]);
    let num3 = Number(arr[2]);

    let sum = num1 + num2 + num3;
    console.log(sum)
}

sum3Numbers(['2', '3', '4']);

function calculateSumAndVAT(arr) {
    let sum = 0;

    for(let price of arr) {
        sum += Number(price);
    }
	
	let vat = sum * 0.2;
	let total = sum * 1.2;

    console.log(`SUM = ${sum}`);
    console.log(`VAT = ${vat}`);
    console.log(`TOTAL = ${total}`);
}

calculateSumAndVAT(['1.20', '2.60', '3.50']);