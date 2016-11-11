
myApp.factory("utilities", function() {
    var service = {};

    service.insertSortedDecending = function(list, item, propertyName) {
        var count = 0;
        while (count < list.length) {
            if (list[count][propertyName] <= item[propertyName]) {
                list.splice(count, 0, item);
                return;
            }
            count ++;
        }
        list.push(item);
    }

    service.insertSortedDate = function(list, item, propertyName) {
        var count = 0;
        while (count < list.length) {
            if (moment(list[count][propertyName]).startOf('day') > moment(item[propertyName]).startOf('day')) {
                list.splice(count, 0, item);
                return;
            }
            count ++;
        }
        list.push(item);
    }

    service.indexOfDate = function(target, list, propertyName) {
        var lo = 0;
        var hi = list.length - 1;
        while (lo <= hi) {
            var mid = Math.round(lo + (hi - lo) / 2);
            if (target < moment(list[mid][propertyName])) {
                hi = mid - 1;
            }
            else if (target > moment(list[mid][propertyName])) {
                lo = mid + 1;
            }
            else return mid;
        }
        return -1;
    }
    
    return service;
})