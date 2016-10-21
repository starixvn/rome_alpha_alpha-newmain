/// <reference path="./balance-info.ts" />
/// <reference path="./statistics.ts" />
/// <reference path="./notice.ts" />

$(document).ready(() => {
    const balanceInfo = new BalanceInfo($);
    balanceInfo.registerEvent();
})