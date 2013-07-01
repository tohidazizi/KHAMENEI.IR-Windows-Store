/*
 Name: Persian Date
 Developer : Bahram Maravandi
 Lastupdate: 24-12-2007
 Thanks to: Amin Habibi Shahri
 
 Modified by Vahid Nasiri - 12/2008
 */
var WEEKDAYS = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday");
var PERSIAN_MONTHS = ["فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"];
var PERSIAN_DIGITS = ["۰", "۱", "۲", "۳", "۴", " ۵", "۶", "۷", "۸", "۹"];
var PERSIAN_DAYS = ["اورمزد", "بهمن", "اوردیبهشت", "شهریور", "سپندارمذ", "خورداد", "امرداد", "دی به آذز", "آذز", "آبان", "خورشید", "ماه", "تیر", "گوش", "دی به مهر", "مهر", "سروش", "رشن", "فروردین", "بهرام", "رام", "باد", "دی به دین", "دین", "ارد", "اشتاد", "آسمان", "زامیاد", "مانتره سپند", "انارام", "زیادی"];
var PERSIAN_WEEKDAYS = new Array("یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه");
var GREGORIAN_MONTH = ["ژانویه", "فوریه", "مارس", "آوریل", "می", "ژوئن", "جولای", "آگوست", "سپتامبر", "اکتبر", "نوامبر", "دسامبر"];

var GREGORIAN_EPOCH = 1721425.5;
var PERSIAN_EPOCH = 1948320.5;

/*  MOD  --  Modulus function which works for non-integers.  */
function mod(a, b){
    return a - (b * Math.floor(a / b));
}

function jwday(j){
    return mod(Math.floor((j + 1.5)), 7);
}

//  LEAP_GREGORIAN  --  Is a given year in the Gregorian calendar a leap year ?
function leap_gregorian(year){
    return ((year % 4) == 0) &&
    (!(((year % 100) == 0) && ((year % 400) != 0)));
}

//  GREGORIAN_TO_JD  --  Determine Julian day number from Gregorian calendar date
function gregorian_to_jd(year, month, day){
    return (GREGORIAN_EPOCH - 1) +
    (365 * (year - 1)) +
    Math.floor((year - 1) / 4) +
    (-Math.floor((year - 1) / 100)) +
    Math.floor((year - 1) / 400) +
    Math.floor((((367 * month) - 362) / 12) +
    ((month <= 2) ? 0 : (leap_gregorian(year) ? -1 : -2)) +
    day);
}

//  JD_TO_GREGORIAN  --  Calculate Gregorian calendar date from Julian day
function jd_to_gregorian(jd){
    var wjd, depoch, quadricent, dqc, cent, dcent, quad, dquad, yindex, dyindex, year, yearday, leapadj;
    
    wjd = Math.floor(jd - 0.5) + 0.5;
    depoch = wjd - GREGORIAN_EPOCH;
    quadricent = Math.floor(depoch / 146097);
    dqc = mod(depoch, 146097);
    cent = Math.floor(dqc / 36524);
    dcent = mod(dqc, 36524);
    quad = Math.floor(dcent / 1461);
    dquad = mod(dcent, 1461);
    yindex = Math.floor(dquad / 365);
    year = (quadricent * 400) + (cent * 100) + (quad * 4) + yindex;
    if (!((cent == 4) || (yindex == 4))) {
        year++;
    }
    yearday = wjd - gregorian_to_jd(year, 1, 1);
    leapadj = ((wjd < gregorian_to_jd(year, 3, 1)) ? 0 : (leap_gregorian(year) ? 1 : 2));
    month = Math.floor((((yearday + leapadj) * 12) + 373) / 367);
    day = (wjd - gregorian_to_jd(year, month, 1)) + 1;
    
    return new Array(year, month, day);
}

//  LEAP_PERSIAN  --  Is a given year a leap year in the Persian calendar ?
function leap_persian(year){
    return ((((((year - ((year > 0) ? 474 : 473)) % 2820) + 474) + 38) * 682) % 2816) < 682;
}

//  PERSIAN_TO_JD  --  Determine Julian day from Persian date
function persian_to_jd(year, month, day){
    var epbase, epyear;
    
    epbase = year - ((year >= 0) ? 474 : 473);
    epyear = 474 + mod(epbase, 2820);
    
    return day +
    ((month <= 7) ? ((month - 1) * 31) : (((month - 1) * 30) + 6)) +
    Math.floor(((epyear * 682) - 110) / 2816) +
    (epyear - 1) * 365 +
    Math.floor(epbase / 2820) * 1029983 +
    (PERSIAN_EPOCH - 1);
}

//  JD_TO_PERSIAN  --  Calculate Persian date from Julian day

function jd_to_persian(jd){
    var year, month, day, depoch, cycle, cyear, ycycle, aux1, aux2, yday;
    
    
    jd = Math.floor(jd) + 0.5;
    
    depoch = jd - persian_to_jd(475, 1, 1);
    cycle = Math.floor(depoch / 1029983);
    cyear = mod(depoch, 1029983);
    if (cyear == 1029982) {
        ycycle = 2820;
    }
    else {
        aux1 = Math.floor(cyear / 366);
        aux2 = mod(cyear, 366);
        ycycle = Math.floor(((2134 * aux1) + (2816 * aux2) + 2815) / 1028522) +
        aux1 +
        1;
    }
    year = ycycle + (2820 * cycle) + 474;
    if (year <= 0) {
        year--;
    }
    yday = (jd - persian_to_jd(year, 1, 1)) + 1;
    month = (yday <= 186) ? Math.ceil(yday / 31) : Math.ceil((yday - 6) / 30);
    day = (jd - persian_to_jd(year, month, 1)) + 1;
    return new Array(year, month, day);
}

function calcPersian(year, month, day){
    var date, j;
    
    j = persian_to_jd(year, month, day);
    date = jd_to_gregorian(j);
    weekday = jwday(j);
    return new Array(date[0], date[1], date[2], weekday);
}

//  calcGregorian  --  Perform calculation starting with a Gregorian date
function calcGregorian(year, month, day){
    month--;
    
    var j, weekday;
    
    //  Update Julian day
    
    j = gregorian_to_jd(year, month + 1, day) +
    (Math.floor(0 + 60 * (0 + 60 * 0) + 0.5) / 86400.0);
    
    //  Update Persian Calendar
    perscal = jd_to_persian(j);
    weekday = jwday(j);
    return new Array(perscal[0], perscal[1], perscal[2], weekday);
}

function getTodayGregorian(){
    var t = new Date();
    var today = new Date();
    
    var y = today.getYear();
    //var y = 2008;
    if (y < 1000) {
        y += 1900;
    }
    //var postdate = y + "/08/23";
    
    return new Array(y, today.getMonth() + 1, today.getDate(), t.getDay());
}

function getTodayPersian(){
    var t = new Date();
    var today = getTodayGregorian();
    
    var persian = calcGregorian(today[0], today[1], today[2]);
    return new Array(persian[0], persian[1], persian[2], t.getDay());
}

var gDateOnly = false;
// date and time
var gTimeOnly = false;
// time
var gBrakeFormat = false;
// brake and bold the weekday
var gLongFormat = false;
// date and persian day 
var gParsiLongFormat = false;
var gParsiShortFormat = false;
//Converts a gregorian date to Jalali date for different formats
function ToPersianDate(gd){
    //alert(gd.getFullYear());
    var pa = calcGregorian(gd.getFullYear(), gd.getMonth() + 1, gd.getDate());
    var p;
    
    if (gTimeOnly) {
    
        var h = formatPersian(((gd.getHours() > 12) ? (gd.getHours() - 12) : (gd.getHours() === 0 ? 12 : gd.getHours())));
        var min = formatPersian(((gd.getMinutes() < 10) ? ('0' + gd.getMinutes()) : (gd.getMinutes())));
        
        p = h + ':' + min.replace(' ', '') + ' ' +
        ((gd.getHours() >= 12) ? 'ب ظ' : 'ق ظ');
    }
    else {
        if (gBrakeFormat) {
            p = "<b>" + PERSIAN_WEEKDAYS[pa[3]] + "</b><br/>" + formatPersian(pa[2]) + " " + PERSIAN_MONTHS[pa[1] - 1] + " " + formatPersian(pa[0]);
        }
        else {
            if (!gDateOnly) 
                p = PERSIAN_WEEKDAYS[pa[3]] + "،  " + formatPersian(pa[2]) + " " + PERSIAN_MONTHS[pa[1] - 1] + " " + formatPersian(pa[0]);
            else 
                p = PERSIAN_WEEKDAYS[pa[3]] + "،  " + formatPersian(pa[1]) + "/" + formatPersian(pa[2]);
            
            if (gLongFormat) {
                p += '  ';
                
                var h = formatPersian(((gd.getHours() > 12) ? (gd.getHours() - 12) : (gd.getHours() === 0 ? 12 : gd.getHours())));
                var min = formatPersian(((gd.getMinutes() < 10) ? ('0' + gd.getMinutes()) : (gd.getMinutes())));
                
                p += '،  ' + h + ':' + min.replace(' ', '') + ' ' +
                ((gd.getHours() >= 12) ? 'ب ظ' : 'ق ظ');
            }
            
            if (gParsiLongFormat) {
                p += '،  ' + PERSIAN_DAYS[pa[2] - 1] + ' روز';
            }
            
            if (gParsiShortFormat) {
                p = formatPersian(pa[2]) +
                " " +
                PERSIAN_MONTHS[pa[1] - 1] +
                " " +
                formatPersian(pa[0]);
            }
        }
    }
    return "<span dir='rtl'>" + p + appendEnDate(gd) + "</span>";
    
}

function ArrayToPersianDate(pa){
    // calc weekday
    var pd = calcPersian(pa[0], pa[1], pa[2]);
    var gd = calcGregorian(pd[0], pd[1], pd[2]);
    var p = PERSIAN_WEEKDAYS[gd[3]] + "،  " + formatPersian(gd[2]) + " " + PERSIAN_MONTHS[gd[1] - 1] + " " + formatPersian(gd[0]);
    
    return p;
    
}

function ArrayToGregorianDate(pa){
    // calc weekday
    var pd = calcPersian(pa[0], pa[1], pa[2]);
    var p = pd[0] + "-" + ((pd[1] < 10) ? "0" + pd[1] : pd[1]) + "-" + ((pd[2] < 10) ? "0" + pd[2] : pd[2]);
    
    return p;
    
}

function ToPersianDateLong(gdStr){

    var origData = gdStr;
    if (window.location.host.toLowerCase().indexOf("mail.yahoo.com") != -1) {
        var idx = gdStr.lastIndexOf("/");
        if (idx != -1) {
            var year = parseInt(gdStr.substr(idx + 1, 2), 10);
            if (!isNaN(year)) {
                year += 2000; //Yahoo's year fix
                //12/15/08 PM --> to
                //12/15/2008 PM        
                gdStr = gdStr.substr(0, idx + 1) + year + gdStr.substr(idx + 3, gdStr.length - 3);
            }
        }
    }
    
    var gd = new Date(gdStr);
    
    var hrs = gd.getHours();
    if (isNaN(hrs)) 
        return origData;
    
    var mins = gd.getMinutes();
    if (isNaN(mins)) 
        return origData;
    
    var h = formatPersian(((hrs > 12) ? (hrs - 12) : (hrs === 0 ? 12 : hrs)));
    var min = formatPersian(((mins < 10) ? ('0' + mins) : (mins)));
    var pa = calcGregorian(gd.getFullYear(), gd.getMonth() + 1, gd.getDate());
    return "<span style='font-family:tahoma; font-size:8pt;'  dir='rtl'>" +
    formatPersian(pa[2]) +
    " " +
    PERSIAN_MONTHS[pa[1] - 1] +
    " " +
    formatPersian(pa[0]) +
    '،  ' +
    h +
    ':' +
    min.replace(' ', '') +
    ' ' +
    ((hrs >= 12) ? 'ب ظ' : 'ق ظ') +
    appendEnDate(gd) +
    "</span>";
}


//
// formats numerical values to persian numbers
// added by Bahram Maravandi
// 13 Feb 2007,  24 Bahman 1385
//
function formatPersian(num){
    var tmp = num;
    tmp = tmp.toString();
    
    for (var i = 0; i < 10; i++) {
        for (var z = 0; z < tmp.length; z++) 
            tmp = tmp.replace(i, PERSIAN_DIGITS[i]);
    }
    
    tmp = tmp.replace(" ", '');
    return tmp;
}

//Added by Vahid Nasiri for Blogger ...

function appendEnDate(dataStr){
    var curdate = new Date(dataStr);
    var day = curdate.getDate();
    var month = curdate.getMonth();
    var year = curdate.getFullYear();
    if (!(isNaN(day) && isNaN(month))) 
        return ' (' + formatPersian(day) + ' ' + GREGORIAN_MONTH[month] + ' ' + formatPersian(year) + ') ';
    else 
        return '';
}

var years = new Array(30);
var yearCnt = 0;
var addData = false;
var yearsLastActiveMonth = new Array(30);
function getBloggerPMonthNames(data){
    if (isNaN(data)) {
        if (addData) {
            yearsLastActiveMonth[yearCnt - 1] = data;
            addData = false;
        }
        var gd = new Date(" 1 " + data + " " + new Date().getYear());
        var pa = calcGregorian(gd.getFullYear(), gd.getMonth() + 1, gd.getDate());
        return PERSIAN_MONTHS[pa[1] - 1];
    }
    else {
        years[yearCnt] = data;
        yearCnt++;
        addData = true;
    }
}

var modYear = 0;
function getBloggerPYear(data){
    if (!isNaN(data)) {
        var myDate = " 1 " + yearsLastActiveMonth[modYear] + " " + years[modYear];
        var gd = new Date(myDate);
        modYear++;
        var pa = calcGregorian(gd.getFullYear(), gd.getMonth() + 1, gd.getDate());
        return formatPersian(pa[0]) + "/" + formatPersian(data);
    }
    else 
        return data;
}
