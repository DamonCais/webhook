const TRUEOBJ = {
  errMsg: ""
};

let isEmptyStr = function(val) {
  if (typeof val === "string") {
    return val.trim() === "";
  } else {
    return !val && val !== 0;
  }
};

function validateEmpty(val, bool) {
  if (!bool) {
    return TRUEOBJ;
  }
  if (isEmptyStr(val)) {
    return {
      errMsg: "This is require field"
    };
  } else {
    return TRUEOBJ;
  }
}

function validateNumber(val, bool) {
  if (!bool) {
    return TRUEOBJ;
  }
  let reg = /^[1-9]\d*(\.)?\d*|0\.\d*[1-9]\d*$/;
  if (isEmptyStr(val)) {
    return {
      errMsg: "This is require field"
    };
  } else if (val <= 0) {
    return {
      errMsg: "Please enter a value greater than 0! "
    };
  } else if (!reg.test(val)) {
    return {
      errMsg: "This field should be a num"
    };
  } else {
    return TRUEOBJ;
  }
}

function validateEmail(val, bool) {
  let reg = /^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.[a-zA-Z0-9]{2,6}$/;

  if (!bool) {
    return TRUEOBJ;
  }
  if (isEmptyStr(val)) {
    return {
      errMsg: "This is require field"
    };
  } else if (!reg.test(val)) {
    return {
      errMsg: "This field should be a Email"
    };
  } else {
    return TRUEOBJ;
  }
}

function validatePhone(val, bool) {
  let reg = /^[1][3,4,5,7,8][0-9]{9}$/;
  if (!bool) {
    return TRUEOBJ;
  }
  if (isEmptyStr(val)) {
    return {
      errMsg: "This is require field"
    };
  } else if (!reg.test(val)) {
    return {
      errMsg: "This field should be a Phone"
    };
  } else {
    return TRUEOBJ;
  }
}

function validateChinaId(val, bool) {
  var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
  if (!bool) {
    return TRUEOBJ;
  }
  if (isEmptyStr(val)) {
    return {
      errMsg: "This is require field"
    };
  } else if (!reg.test(val)) {
    return {
      errMsg: "This field should be a ChinaId"
    };
  } else {
    return TRUEOBJ;
  }
}

function validatePassport(val, bool) {
  var reg1 = /^[a-zA-Z]{5,17}$/;
  var reg2 = /^[a-zA-Z0-9]{5,17}$/;
  if (!bool) {
    return TRUEOBJ;
  }
  if (isEmptyStr(val)) {
    return {
      errMsg: "This is require field"
    };
  } else if (!reg1.test(val) && !reg2.test(val)) {
    return {
      errMsg: "This field should be a Passport"
    };
  } else {
    return TRUEOBJ;
  }
}

function validateHongKongPermit(val, bool) {
  var reg = /^[HMhm]{1}([0-9]{10}|[0-9]{8})$/;
  if (!bool) {
    return TRUEOBJ;
  }
  if (isEmptyStr(val)) {
    return {
      errMsg: "This is require field"
    };
  } else if (!reg.test(val)) {
    return {
      errMsg: "This field should be a HongKongPermit"
    };
  } else {
    return TRUEOBJ;
  }
}

function validateTaiwanPermit(val, bool) {
  var reg2 = /^[0-9]{8}$/;
  var reg1 = /^[0-9]{10}$/;
  if (!bool) {
    return TRUEOBJ;
  }
  if (isEmptyStr(val)) {
    return {
      errMsg: "This is require field"
    };
  } else if (!reg1.test(val) && !reg2.test(val)) {
    return {
      errMsg: "This field should be a TaiwanPermit"
    };
  } else {
    return TRUEOBJ;
  }
}

export default {
  validateChinaId,
  validateEmail,
  validateEmpty,
  validateHongKongPermit,
  validateNumber,
  validatePassport,
  validatePhone,
  validateTaiwanPermit
};
