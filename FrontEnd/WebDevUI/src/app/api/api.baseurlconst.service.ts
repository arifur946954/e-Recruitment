export class ApiConst {
  constructor() { }
  IsLive: boolean = false;
  autohost(location) {
    debugger;
    var apiHost = '';
    if (this.IsLive) {
      apiHost = 'https://app.citygroupbd.com/erecruite_api/api/'//Real Live
    }
    else {
      apiHost = 'http://localhost:49942/api/'; //Local
    }

    //var apiHost='http://192.168.61.246:81/api/'; //Live

    return apiHost;
  }
}