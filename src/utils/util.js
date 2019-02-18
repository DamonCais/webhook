function getBatteryText(index) {
    let obj = {}
    switch (index) {
        case 0:
            obj = {
                text: '不可用',
                icon: 'md-battery-charging',
                type: 'error'
            }
            break
        case 1:
            obj = {
                text: '可用',
                icon: 'md-battery-full',
                type: 'my-battery'
            }
            break
        case 2:
            obj = {
                text: '空单元',
                icon: 'md-square-outline',
                type: 'default'
            }
            break
        default: obj = {
            text: '异常单元',
            icon: 'md-warning',
            type: 'warning'
        }
    }
    console.log(obj)
    return obj
}
module.exports = {
    getBatteryText: getBatteryText,
}
