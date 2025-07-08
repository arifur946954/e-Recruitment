import { Component, OnInit, Inject, ViewChild, TemplateRef, ElementRef, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators, NgForm, FormArray } from '@angular/forms';
import { DOCUMENT, formatDate, ViewportScroller } from '@angular/common';
import { MatDialog, MatDialogRef, MatDialogConfig } from '@angular/material/dialog';
import { Options } from 'select2';
import { fontModel } from './fontModel';
import { jsPDF } from 'jspdf';
import { Console } from 'console';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/api/api.service';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/api/user';
import { CommonService } from 'src/app/theme/components/commonservice/commonservice.component';
import { CommonPager } from 'src/app/theme/components/commonpager/commonpager';
import { ReportViewer } from '../reportviewer/reportviewer';
import { Settings } from 'src/app/app.settings.model';
import { AppSettings } from 'src/app/app.settings';
import { pathValidation } from 'src/app/api/api.pathvlidation.service';
import { DataService } from 'src/app/api/api.dataservice.service';
import { PagerService } from 'src/app/api/api.pager.service';
import { Conversion } from 'src/app/api/api.conversion.service';
import { it } from 'date-fns/locale';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

declare var $: any;

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.scss'],
   // providers: [PagerService]
    providers: [Conversion]
})

export class ProfileComponent implements OnInit {
  @ViewChild('cmnsrv', { static: false }) _msg: CommonService;
  @ViewChild('cmnpager', { static: false }) _pg: CommonPager;
  @ViewChild(ReportViewer) _rptViewer: ReportViewer;
  public settings: Settings;
  public options: Options;
  private userID = sessionStorage.getItem("userID");
  public userRole=sessionStorage.getItem("rolename");
  public loggedUserId: string = sessionStorage.getItem("userID");
  public cmnEntity: any = {};
  public isToggleMaster: boolean = true;
  public res: any;
  public resmessage: string;
  public requirementForm: FormGroup;
  public pageSize: number = 10;
  public listJobPost: any;
  public itemListByPage: any = [];
  public jobPostList:any=[];
  public JobIdList:any=[];
  public appliedJobIds: string[] = [];
  public skillList:any;
  public benifitList:any;
  public requirementList:any;
  public otherRequirementList:any;
  public responsibilityList:any;
  public masterList:any;
  public accQlfList:any;
  public wrkExpList:any;
  public proCirtificateList:any;
  public masterDiv:boolean=true;
  public detailDiv:boolean=false;
  public applyForm:boolean=false
  public downloadCvPath:any;
  public downloadNidPath:any;
  public downloadTinPath:any;
  public updateOidEdit:any;
  public imageId:any;
  public signatureId:any;
  public cvId:any;
  public tinId:any;
  public NIDId:any;


  bloodGroupList:string[]=['A+','B+','O+',"AB+",'A-','B-','O-',"AB-"];
  qualificationType: string[] = ['SSC/Equivalent', 'HSC/Deploma', 'BSC', 'MSC', 'Other']; 
  WorkOrderStatus: Array<{ value: string, label: string }> = [
    { value: '1', label: 'Work Order InComing' },
    { value: '0', label: 'Work Order  OutGoing' },
  ];
  genderList:string[]=['Male','Female','Other'];
  maritialStatusList:string[]=['Married','Unmarried'];
  religionList: string[] = ['Islam', 'Hindu', 'Christianity', 'Buddhism', 'Other'];
  image: FileList | null = null;
  signature: FileList | null = null;
  cv: FileList | null = null;
  nid: FileList | null = null;
  tin: FileList | null = null;
  selectedFiles:FileList | null = null;
  binaryString:any
  public preDivList: any = [];
  public parDivList: any = [];
  public preDisList: any = [];
  public parDisList: any = [];
  public preThnList: any = [];
  public parThnList: any = [];
  public jobpostId:string;
  imageUrls: SafeUrl[] = [];
  signatureUrls: SafeUrl[] = [];
  public isEdit:boolean=false;
  public isShowmstr:any;
    public alApply:boolean=false;
  



  constructor(public appSettings:AppSettings, 
    private _pathValidation: pathValidation,
    private formBuilder: FormBuilder,
    private viewportScroller: ViewportScroller,
    public fb: FormBuilder, 
    private sanitizer: DomSanitizer,
    private _conversion: Conversion,
    public router:Router, 
    public _apiService : ApiService, 
    private toastr: ToastrService,
    private _dataservice:DataService, 
   
    @Inject(DOCUMENT) private document: any
    ){
      //this.options = this._pathValidation.ngSelect2Option();
     this.settings = this.appSettings.settings;
     this._pathValidation.validate(this.document.location);
     this.cmnEntity = this._pathValidation.rowEntities();
     

 
  }
  

  getNameToNumDate(strDate: string) {
    debugger;
    var nDate = new Date(strDate);
    var Nowdate = nDate.getFullYear() + '-' + ('0' + (nDate.getMonth() + 1)).slice(-2) + '-' + ('0' + nDate.getDate()).slice(-2);
    return Nowdate;
    
}
  ngOnInit(){
     
     this.getUserDetailsById()
     this.getAppplicantProfileById();
    this.getList();
    this.createForm();
    this.addInitialAcademicQualifications();
    this.getAllDivision();
    this.getParAllDivision();
    //this.getListByPage(this.pageSize);
    this.GetJobIdList(this.loggedUserId)
    

  //    this._dataservice.callProfileFunction$.subscribe((data) => {
  //     this.edit(data);
  //   });

  //   this._dataservice.oid$.subscribe(oid => {

  // });
  }
  cmnbtnAction(evmodel) {
    debugger
    this[evmodel.func](evmodel);
}

showHide() {
    debugger;
    this.cmnEntity.isShow ? this.reset() : this.getListByPage(this.pageSize);
}

setToggling(divName) {
  debugger;
  if (divName == 'Master') {
    this.isToggleMaster = this.isToggleMaster ? false : true;
  }


}

//GET DIVISION NAME
public _divUrl: string = 'jobdropdown/getalldivision';
getAllDivision() {
    var list: Array<{ id, text }> = [{ id: 0, text: "Please Select" }];
    var apiUrl = this._divUrl;
    this._dataservice.getall(apiUrl)
        .subscribe(
            response => {
                this.res = response;
                if (this.res.resdata.listAllDiv.length > 0) {
                    var itemList = this.res.resdata.listAllDiv;
                    itemList.forEach(item => {
                        list.push({ id: item.oId, text: item.divName });
                    });
                    this.preDivList = list;
                    console.log(" this.divisonList", this.preDivList)
                }
            }, error => {
                console.log(error);
            });
}

public _parDivUrl: string = 'jobdropdown/getalldivision';
getParAllDivision() {
    var list: Array<{ id, text }> = [{ id: 0, text: "Please Select" }];
    var apiUrl = this._parDivUrl;
    this._dataservice.getall(apiUrl)
        .subscribe(
            response => {
                this.res = response;
                if (this.res.resdata.listAllDiv.length > 0) {
                    var itemList = this.res.resdata.listAllDiv;
                    itemList.forEach(item => {
                        list.push({ id: item.oId, text: item.divName });
                    });
                    this.parDivList = list;
                   
                }
            }, error => {
                console.log(error);
            });
}





public _parDstrictUrl: string = 'jobdropdown/getalldistrictById';
getDistrictById(divId:string) {
  debugger
    var list: Array<{ id, text }> = [{ id: 0, text: "Please Select" }];
    var apiUrl = this._parDstrictUrl;
    if(divId){
      var param=divId.replace(/^\D+/g, '');
    this._dataservice.getbyid(apiUrl,param)
        .subscribe(
            response => {
                this.res = response;
                if (this.res.resdata.listAllDis.length > 0) {
                    var itemList = this.res.resdata.listAllDis;
                    itemList.forEach(item => {
                        list.push({ id: item.oId, text: item.disName });
                    });
                    this.preDisList = list;
                }
            }, error => {
                console.log(error);
            });
            }
}


getParDistrictById(divId:string) {
  debugger
    var list: Array<{ id, text }> = [{ id: 0, text: "Please Select" }];
    var apiUrl = this._parDstrictUrl;
    //var param = { id: divId };
    if(divId){
    var param=divId.replace(/^\D+/g, '');
    this._dataservice.getbyid(apiUrl,param)
        .subscribe(
            response => {
                this.res = response;
                if (this.res.resdata.listAllDis.length > 0) {
                    var itemList = this.res.resdata.listAllDis;
                    itemList.forEach(item => {
                        list.push({ id: item.oId, text: item.disName });
                    });
                    this.parDisList = list;
                }
            }, error => {
                console.log(error);
            });
            }
}





public _getbyThnIdUrl: string = 'jobdropdown/getallthanaById';
getThanaById(disId:string) {
  debugger
    var list: Array<{ id, text }> = [{ id: 0, text: "Please Select" }];
    var apiUrl = this._getbyThnIdUrl;
    var param=disId;
    this._dataservice.getbyid(apiUrl,param)
        .subscribe(
            response => {
                this.res = response;
                console.log("Tahana is...........",this.res)
                if (this.res.resdata.listAllThna.length > 0) {
                    var itemList = this.res.resdata.listAllThna;
                    itemList.forEach(item => {
                        list.push({ id: item.oId, text: item.thanaName });
                    });
                    this.preThnList = list;
                }
            }, error => {
                console.log(error);
            });
}







getParThanaById(disId:string) {
  debugger
    var list: Array<{ id, text }> = [{ id: 0, text: "Please Select" }];
    var apiUrl = this._getbyThnIdUrl;
    //var param = { id: divId };
    var param=disId;
    this._dataservice.getbyid(apiUrl,param)
        .subscribe(
            response => {
                this.res = response;
                console.log("Tahana is...........",this.res)
                if (this.res.resdata.listAllThna.length > 0) {
                    var itemList = this.res.resdata.listAllThna;
                    itemList.forEach(item => {
                        list.push({ id: item.oId, text: item.thanaName });
                    });
                    this.parThnList = list;
                }
            }, error => {
                console.log(error);
            });
}

// public _getparThnIdUrl: string = 'reqform/getthanabyid';
// getParThanaById(divId:string) {
//   var list: Array<{ id, text }> = [{ id: 0, text: "Please Select" }];
//     debugger;
//     var param = { id: divId };
//     var apiUrl = this._getparThnIdUrl
//     this._dataservice.getWithMultipleModel(apiUrl, param)
//         .subscribe(response => {
//             this.res = JSON.parse(response.resdata.thanaList) ;
//             var thana= this.res;
//             thana.forEach(item=>{
//               list.push({id:item.upzlOid,text:item.upzlName})
//             })
//             this.parThnList=list;
//         }, error => {
//             console.log(error);
//         });
// }
//SAME AS PRESENT ADDRESS
sameAsPresentAdd(event: Event){
  const isChecked = (event.target as HTMLInputElement).checked;
  debugger
  if (isChecked) {
  


 this.requirementForm.controls.parAddDivision.setValue(this.requirementForm.controls.preAddDivision.value);
  this.getParDistrictById(this.requirementForm.controls.parAddDivision.value);
    setTimeout(() => {
    this.requirementForm.controls.parAddDistrict.setValue(this.requirementForm.controls.preAddDistrict.value);
  }, 100);

    this.getParThanaById(this.requirementForm.controls.preAddDistrict.value);
    setTimeout(() => {
    this.requirementForm.controls.parAddThana.setValue(this.requirementForm.controls.preAddThana.value);
  }, 200);

  
  this.requirementForm.controls.parAddVillage.setValue(this.requirementForm.controls.preAddVillage.value);
  this.requirementForm.controls.parAddPostOffice.setValue(this.requirementForm.controls.preAddPostOffice.value);
  this.requirementForm.controls.parAddDetai.setValue(this.requirementForm.controls.preAddDetai.value);
  }
  else{
    this.requirementForm.controls.parAddDivision.setValue(null);
    this.requirementForm.controls.parAddDistrict.setValue(null);
    this.requirementForm.controls.parAddThana.setValue(null);
    this.requirementForm.controls.parAddVillage.setValue(null);
    this.requirementForm.controls.parAddPostOffice.setValue(null);
    this.requirementForm.controls.parAddDetai.setValue(null);
  }
}

reset(){

}

createForm() {
   
  this.requirementForm = this.formBuilder.group({
    applicantId: null,
    jobPostId: null,
    jobTitle: new FormControl(null),
    company: new FormControl(null),
    department: new FormControl(null),
    appliedPost: new FormControl(null),
    mobileNumber: new FormControl(null),
    //campaign: new FormControl(null, Validators.required),
    name:new FormControl(null),
    fatherName: new FormControl(null, Validators.required),
    motherName: new FormControl(null, Validators.required),
    nidN: new FormControl(null, Validators.required),
    tin: new FormControl(null),
    dateOfBirth: new FormControl(null, Validators.required),
    birthPlace: new FormControl(null, Validators.required),
    relegion: new FormControl(null),
    bloodGroup: new FormControl(null),
    gender: new FormControl(null, Validators.required),
    maritialStatus: new FormControl(null, Validators.required),
    spouseName: null,
    email: new FormControl(null),
  
    isActive: null,
    preAddDivision: new FormControl(null, Validators.required),
    preAddDistrict: new FormControl(null, Validators.required),
    preAddThana: new FormControl(null, Validators.required),
    preAddPostOffice: new FormControl(null),
    preAddVillage: new FormControl(null),
    preAddDetai: new FormControl(null, Validators.required),

    parAddDivision: new FormControl(null, Validators.required),
    parAddDistrict: new FormControl(null, Validators.required),
    parAddThana: new FormControl(null, Validators.required),
    parAddPostOffice: new FormControl(null),
    parAddVillage: new FormControl(null),
    parAddDetai: new FormControl(null, Validators.required),

    expectedSelery: new FormControl(null),
    appliedBy: new FormControl(null),
   
   
    //companyDeptPost: new FormControl(null),
    //companyDeptPostOpnDate: new FormControl(null),
    //companyDeptPostActvStatus: new FormControl(null),
    imagePath: null,
    signaturePath:null,
    cvPath: null,
    nidPath: null,
    tinPath: null,
    academicQualifications: this.formBuilder.array([]),
    workExperiences: this.formBuilder.array([]), 
    professionalCirtificate: this.formBuilder.array([]),

    
  });
  
}


//for validation

get nidN() {
  return this.requirementForm.get('nidN');
}
get birthPlace() {
  return this.requirementForm.get('birthPlace');
}
get dateOfBirth() {
  return this.requirementForm.get('dateOfBirth');
}
get fatherName() {
  return this.requirementForm.get('fatherName');
}
get motherName() {
  return this.requirementForm.get('motherName');
}
get maritialStatus() {
  return this.requirementForm.get('maritialStatus');
}
get gender() {
  return this.requirementForm.get('gender');
}
get preAddDivision() {
  return this.requirementForm.get('preAddDivision');
}

get preAddDistrict() {
  return this.requirementForm.get('preAddDistrict');
}
get preAddThana() {
  return this.requirementForm.get('preAddThana');
}
get preAddDetai() {
  return this.requirementForm.get('preAddDetai');
}
get parAddDivision() {
  return this.requirementForm.get('parAddDivision');
}
get parAddDistrict() {
  return this.requirementForm.get('parAddDistrict');
}
get parAddThana() {
  return this.requirementForm.get('parAddThana');
}
get parAddDetai() {
  return this.requirementForm.get('parAddDetai');
}
// get board() {
//   return this.requirementForm.get('board');
// }
// get degree() {
//   return this.requirementForm.get('board');
// }
// get institution() {
//   return this.requirementForm.get('institution');
// }
// get major() {
//   return this.requirementForm.get('major');
// }
// get result() {
//   return this.requirementForm.get('result');
// }
// get passingyear() {
//   return this.requirementForm.get('passingyear');
// }



// async onSubmit(): Promise<void> {
//   debugger
//   try {
//     await this.uploadImages();
//     await this.uploadSignature();
//     await this.uploadNID();
//     await this.uploadTin();
//     await this.uploadCV();
//     await this.uploadTin();
//     this.onSubmitFileForm();
//   } catch (error) {
//     console.error("File upload sequence failed.", error);
//   }
// }

async onFormSubmit(): Promise<void> {
  debugger;

  try {
    try {
      await this.uploadImages();
    } catch (error) {
      console.error("uploadImages failed", error);
      this.toastr.error("Image upload is required.")
      //alert("Image upload is required.");
      return; // Stop submission
    }



    try {
      await this.uploadSignature();
    } catch (error) {
      console.error("uploadSignature failed", error);
      this.toastr.error("Signature upload is required.")
      //alert("Signature  upload is required.");
      return; // Stop submission
    }

    try {
      await this.uploadNID();
    } catch (error) {
      console.error("uploadNID failed", error);
    }

        try {
      await this.uploadTin();
    } catch (error) {
      console.error("uploadTin failed", error);
    }

    try {
      await this.uploadCV();
    } catch (error) {
      console.error("uploadCV failed", error);
    }

    // Only after all uploads attempted, submit the form
    this.onSubmitFileForm();

  } catch (error) {
    console.error("Unexpected error in onSubmit", error);
  }
}

async onSubmit(): Promise<void> {
  debugger;

  try {
    try {
      await this.uploadImages();
    } catch (error) {
      console.error("uploadImages failed", error);
      //this.toastr.error("Image upload is required.")
     
    }

    try {
      await this.uploadSignature();
    } catch (error) {
      console.error("uploadSignature failed", error);
      //this.toastr.error("Signature upload is required.")
      //alert("Signature  upload is required.");
      //return; // Stop submission
    }

    try {
      await this.uploadNID();
    } catch (error) {
      console.error("uploadNID failed", error);
    }

    try {
      await this.uploadTin();
    } catch (error) {
      console.error("uploadTin failed", error);
    }

    try {
      await this.uploadCV();
    } catch (error) {
      console.error("uploadCV failed", error);
    }

    // Only after all uploads attempted, submit the form
    this.onSubmitFileForm();

  } catch (error) {
    console.error("Unexpected error in onSubmit", error);
  }
}



// onFileChange(event: any): void {
//   this.image = event.target.files;  
// }
onFileChange(event: any): void {
  const files = event.target.files;

  if (files && files.length > 0) {
    const file: File = files[0];

    const allowedTypes = [
      'image/png',
      'image/jpeg',
      'image/jpg',
      'image/gif',
      'image/bmp',
      'image/x-png'
    ];

    if (!allowedTypes.includes(file.type)) {
      alert('Only PNG, JPEG, JPG, GIF, BMP images are allowed.');
      event.target.value = ''; // clear the file input
      return;
    }

    this.image = files; // valid file, store for upload
  }
}

OnSignatureChange(event: any): void {
  const files = event.target.files;

  if (files && files.length > 0) {
    const file: File = files[0];

    const allowedTypes = [
      'image/png',
      'image/jpeg',
      'image/jpg',
      'image/gif',
      'image/bmp',
      'image/x-png'
    ];

    if (!allowedTypes.includes(file.type)) {
      alert('Only PNG, JPEG, JPG, GIF, BMP images are allowed.');
      event.target.value = ''; // clear the file input
      return;
    }

    this.signature = files; // valid file, store for upload
  }
}



onTINChange(event: any): void {
  this.tin = event.target.files;  
}

onNIDChange(event: any): void {
  this.nid = event.target.files;  
}

// OnSignatureChange(event: any): void {
//   this.signature = event.target.files;  
// }

OnCvChange(event: any): void {
  this.cv = event.target.files;  
}

// Upload image with async/await
public imgUrl:string='reqform/dbsinleuploadfile';
async uploadImages(): Promise<void> {
  debugger
  return new Promise((resolve, reject) => {
    this._dataservice.uploadFile(this.image,this.imgUrl).subscribe(
      response => {
        this.requirementForm.controls.imagePath.setValue(response.data[0]);
        console.log(" this.requirementForm.controls.imagePath", this.requirementForm.controls.imagePath)
        resolve(); 
      },
      error => {
        console.error(error);
        alert('Image upload failed. Please try again.');
        reject(error); 
      }
    );
  });
}

// Upload signature with async/await
public sigUrl:string='reqform/dbsinleuploadfile';
async uploadSignature(): Promise<void> {
  debugger
  return new Promise((resolve, reject) => {
    this._dataservice.uploadFile(this.signature,this.sigUrl).subscribe(
      response => {
        this.requirementForm.controls.signaturePath.setValue(response.data[0]);
        resolve(); 
      },
      error => {
        console.error(error);
        alert('Signature upload failed. Please try again.');
        reject(error); 
      }
    );
  });
}

// Upload CV with async/await
public cvUrl:string='reqform/dbsinleuploadfile';
async uploadCV(): Promise<void> {
  debugger
  return new Promise((resolve, reject) => {
      if (!this.cv ) {
      console.warn('No TIN file selected.');
      resolve(); // Or reject() if you want to treat this as a failure
      return;
    }
    this._dataservice.uploadFile(this.cv,this.cvUrl).subscribe(
      response => {
        this.requirementForm.controls.cvPath.setValue(response.data[0]);
        resolve(); 
      },
      error => {
        console.error(error);
        alert('CV upload failed. Please try again.');
        reject(error); 
      }
    );
  });
}

// Upload NID with async/await
public nidUrl:string='reqform/dbsinleuploadfile';
async uploadNID(): Promise<void> {
  debugger
  return new Promise((resolve, reject) => {
     if (!this.nid ) {
      console.warn('No TIN file selected.');
      resolve(); // Or reject() if you want to treat this as a failure
      return;
    }
    this._dataservice.uploadFile(this.nid,this.nidUrl).subscribe(
      response => {
        this.requirementForm.controls.nidPath.setValue(response.data[0]);
        console.log("after update nid is ",this.requirementForm.controls.nidPath);
        resolve(); 
      },
      error => {
        console.error(error);
        alert('CV upload failed. Please try again.');
        reject(error); 
      }
    );
  });
}

// Upload TIN with async/await
public tinUrl:string='reqform/dbsinleuploadfile';
async uploadTin(): Promise<void> {
  debugger
  return new Promise((resolve, reject) => {
     if (!this.tin) {
      console.warn('No TIN file selected.');
      resolve(); // Or reject() if you want to treat this as a failure
      return;
    }

    this._dataservice.uploadFile(this.tin,this.tinUrl).subscribe(
      response => {
        this.requirementForm.controls.tinPath.setValue(response.data[0]);
         console.log("after update TIN is ",this.requirementForm.controls.tinPath);
        resolve(); 
      },
      error => {
        console.error(error);
        alert('CV upload failed. Please try again.');
        reject(error); 
      }
    );
  });
}



// Assume this.imageToUpdate is of type File and this.documentId is the ID to update
public uImgUrl:string='reqform/updateFile';
  async updateImage(): Promise<void> {
    debugger
      if (!this.imageId && this.image) {
          this.uploadImages();
           return; 
          }
    try {
      const response = await this._dataservice.updateFile(this.imageId, this.image,this.uImgUrl).toPromise();
      
      if (response.responseCode === 200) {
        this.requirementForm.controls.imagePath.setValue(response.data?.[0] || null);
        console.log(" this.requirementForm.controls.imagePath.setValue", this.requirementForm.controls.imagePath.setValue)
      } else {
      }
    } catch (error) {
      console.error('Update image error:', error);
    }
  }
    
  public uSigUrl:string='reqform/updateFile';
    async updateSignature(): Promise<void> {
      debugger
         if (!this.signatureId && this.signature) {
          this.uploadSignature();
           return; 
          }
    try {
      const response = await this._dataservice.updateFile(this.signatureId, this.signature,this.uSigUrl).toPromise();
      
      if (response.responseCode === 200) {
        this.requirementForm.controls.signaturePath.setValue(response.data?.[0] || null);
      } else {
      }
    } catch (error) {
      console.error('Update image error:', error);
    }
  }

  public uCvUrl:string='reqform/updateFile';
   async updateCV(): Promise<void> {
    debugger
    if (!this.cvId && this.cv) {
          this.uploadCV();
           return; 
          }
    try {
      const response = await this._dataservice.updateFile(this.cvId, this.cv,this.uCvUrl).toPromise();
      
      if (response.responseCode === 200) {
        this.requirementForm.controls.cvPath.setValue(response.data?.[0] || null);
      } else {
      }
    } catch (error) {
      console.error('Update image error:', error);
    }
  }

   public uTinUrl:string='reqform/updateFile';
     async updateTIN(): Promise<void> {
      debugger
       if (!this.tinId && this.tin) {
          this.uploadTin();
           return; 
          }
    try {
      const response = await this._dataservice.updateFile(this.tinId, this.tin,this.uTinUrl).toPromise();
      
      if (response.responseCode === 200) {
        this.requirementForm.controls.tinPath.setValue(response.data?.[0] || null);
      } else {
      }
    } catch (error) {
      console.error('Update image error:', error);
    }
  }

   public uNIDUrl:string='reqform/updateFile';
       async updateNID(): Promise<void> {
        debugger
       if (!this.NIDId && this.nid) {
          this.uploadTin();
           return; 
          }
    try {
      const response = await this._dataservice.updateFile(this.NIDId, this.nid,this.uNIDUrl).toPromise();
      
      if (response.responseCode === 200) {
        this.requirementForm.controls.nidPath.setValue(response.data?.[0] || null);
      } else {
      }
    } catch (error) {
      console.error('Update image error:', error);
    }
  }

// async updateNID(): Promise<void> {
//   try {
//     // If NIDId is not set, upload first
//     if (!this.NIDId && this.nid) {
//       await this.uploadNID();  // Wait for upload to finish
//     }
//     const response = await this._dataservice.updateFile(this.NIDId, this.nid).toPromise();

//     if (response.responseCode === 200) {
//       this.requirementForm.controls.nidPath.setValue(response.data?.[0] || null);
//     }
//   } catch (error) {
//     console.error('Update image error:', error);
//   }
// }





//Save form
public _saveUrl: string = 'reqform/saveupdate';
onSubmitFileForm(): void {
  debugger
  let formValues = { ...this.requirementForm.value };
  delete formValues.academicQualifications;
  delete formValues.workExperiences;
  delete formValues.professionalCirtificate;
  console.log("formValues",formValues)
  const reqform = formValues;
  const acaQlf = this.academicQualifications.value;
  const wrkExp = this.workExperiences.value;
  const profCirtificate = this.professionalCirtificate.value;
  
  const param = { 
    loggedUserId: this.userID,
    strId: this.requirementForm.controls.applicantId.value, 
    strId2: this.userID ,
    JobOid:''
  };
 
  const ModelsArray = [param, [reqform], acaQlf, wrkExp,profCirtificate];
  if (this.requirementForm.invalid) {
    this._msg.error("form is invalid");
    this.requirementForm.markAllAsTouched(); 
    this.academicQualifications.controls.forEach(control => control.markAllAsTouched());
    this.workExperiences.controls.forEach(control => control.markAllAsTouched());
    this.professionalCirtificate.controls.forEach(control => control.markAllAsTouched());// Highlights all invalid fields
    return;
  }else{
  this._dataservice.postMultipleModel(this._saveUrl, ModelsArray)
    .subscribe(response => {
      this.res = response;
      this.resmessage = this.res.resdata.message;
      if (this.res.resdata.resstate) {
        this._msg.success(this.resmessage);
        window.location.reload()
      }
    }
    , error => {
      console.log(error);
    }
  );
}
}


get academicQualifications(): FormArray {
  return this.requirementForm.get('academicQualifications') as FormArray;
}

// Add initial academic qualifications
addInitialAcademicQualifications() {
  for (let i = 0; i < 3; i++) {
    this.addAcademicQualification();
  }
}

// Add a new academic qualification FormGroup to the FormArray
addAcademicQualification() {
  const qualificationGroup = this.formBuilder.group({
    acQlfId: null,
    applicantId: null,
    degree: ['', Validators.required],
    board: ['', Validators.required],
    institution: ['', Validators.required],
    major: ['', Validators.required],
    result:['', Validators.required],
    passingyear: ['', [Validators.required, Validators.pattern("^[0-9]{4}$")]]
  });

  this.academicQualifications.push(qualificationGroup);
}

// Remove an academic qualification from the FormArray
removeAcademicQualification(index: number) {
  if (this.academicQualifications.length > 3) { // Ensure at least 3 qualifications remain
    this.academicQualifications.removeAt(index);
  }
}


get workExperiences(): FormArray {
  return this.requirementForm.get('workExperiences') as FormArray;
}

// Add new work experience form group
addExperience() {
  const experienceGroup = this.formBuilder.group({
    expId: null,
    applicantId: null,
    companyName: [null, Validators.required],
    companyType: [null, Validators.required],
    priodFromDate: [null, Validators.required],
    priodToDate: null,
    jobDescription: [null, Validators.required],
    department: [null, Validators.required],
    designation: [null, Validators.required],
    jobLocation: [null]
  });

  this.workExperiences.push(experienceGroup);
}

// Remove work experience by index
removeExperience(index: number) {
  this.workExperiences.removeAt(index);
}

// Add Proffesional Cirtificate

get professionalCirtificate(): FormArray {
  return this.requirementForm.get('professionalCirtificate') as FormArray;
}
addProfCirtificate() {
  const profCirtificateGroup = this.formBuilder.group({
    pCirtificateId: null,
    applicantId: null,
    courseName: [null, Validators.required],
    instution: [null, Validators.required],
    duration: [null, Validators.required],
    achievementDate: [null, Validators.required]
   
  });

  this.professionalCirtificate.push(profCirtificateGroup);
}

// Remove work experience by index
removeProfCirtificate(index: number) {
  this.professionalCirtificate.removeAt(index);
}







public _getbyListIdUrl: string = 'jobpost/getbylist';
getList() {
    var param = {strid:this.userID};
    var apiUrl = this._getbyListIdUrl
    this._dataservice.getWithMultipleModel(apiUrl, param)
        .subscribe(response => {
            this.res = response;
            this.jobPostList = JSON.parse(this.res.resdata.listJobPost);
            console.log("this.Total data LLLLLLLLLLLLLLLLLLLLLLLLLLL ", this.jobPostList)
        
        }, error => {
            console.log(error);
        });
}

//CHECK USERID AND JOBPOST ID 

public masterListDet:any;
  public _getJobIdList: string = 'reqform/getJobIdByMail';
  GetJobIdList(id) {
      debugger;
      var param = { strId: id };
      var apiUrl = this._getJobIdList
      this._dataservice.getWithMultipleModel(apiUrl, param)
          .subscribe(response => {
              this.res = response;
              if(this.res.resdata.jobIDList){
                this.JobIdList=JSON.parse(this.res.resdata.jobIDList)
                 this.appliedJobIds = this.JobIdList.map(item => item.jObId);
             
              }
             
          }, error => {
              console.log(error);
          });
  }


backToFirst(){
  this.masterDiv=true;
  this.applyForm=false;
}



public masterListDetails:any;
  public _getbyIdUrl: string = 'jobpost/getbyid';
  showDetails(modelEvnt,isShow) {
    this.isShowmstr=isShow;
      debugger;
      console.log("modelEvnt",modelEvnt)
      var param = { strId: modelEvnt.jobOid, strId2: modelEvnt.jobOid };
      var apiUrl = this._getbyIdUrl
      this._dataservice.getWithMultipleModel(apiUrl, param)
          .subscribe(response => {
            this.masterDiv=false;
              this.res = response;
              this.detailDiv=true;
              this.masterList=JSON.parse(this.res.resdata.jobPostMaster)
              this.masterListDetails=this.masterList[0];
              console.log("this.Total test test -------------------",(this.masterListDetails))
              this.skillList=JSON.parse(this.res.resdata.jobSkill)
              this.benifitList=JSON.parse(this.res.resdata.jobBenefit)
              this.requirementList=JSON.parse(this.res.resdata.jobRequirement)
              this.otherRequirementList=JSON.parse(this.res.resdata.jobOtherRequirement)
              this.responsibilityList=JSON.parse(this.res.resdata.jobResponsibility)
              console.log("this.Total data -------------------",(this.masterList))
              console.log("this.Total skillList ",(this.skillList))
              console.log("this.Total benifitList ",(this.benifitList))
              console.log("this.Total requirementList ",(this.requirementList))
              console.log("this.Total otherRequirementList ",(this.otherRequirementList))
              console.log("this.Total responsibilityList ",(this.responsibilityList))

        
           

              console.log("this.this.jobPostForm",this.requirementForm)
          
          }, error => {
              console.log(error);
          });
  }



  
showHtml(){

}
 

ApplyForm(jobPostDetails:any){
  debugger
  this.getUserDetailsById()
  this.masterDiv=false;
  this.detailDiv=false;
  this.applyForm=true;

  this.requirementForm.controls.jobTitle.setValue(jobPostDetails.jobTitle)
  this.requirementForm.controls.company.setValue(jobPostDetails.company)
  this.requirementForm.controls.department.setValue(jobPostDetails.department)
  this.requirementForm.controls.appliedPost.setValue(jobPostDetails.post)
  this.requirementForm.controls.jobPostId.setValue(jobPostDetails.jbPostId)
 
}




public _getbyUserDetailsUrl: string = 'reqform/getuserdetialsbyid';
getUserDetailsById() {
  debugger
    var param = { strId: this.loggedUserId };
    var apiUrl = this._getbyUserDetailsUrl
    this._dataservice.getWithMultipleModel(apiUrl, param)
        .subscribe(response => {
             this.res = JSON.parse(response.resdata.userDetails);
             console.log(" this.res    this.resc  this.res ", this.res)
             this.requirementForm.controls.email.setValue( this.res[0].email)
             this.requirementForm.controls.name.setValue( this.res[0].fullName)
             this.requirementForm.controls.mobileNumber.setValue( this.res[0].mobileNumber)
        }, error => {
            console.log(error);
        });
      }

      //GET APPLICANT BY EMAIL
      public applicantdetail:string
      public _getbyApplicantUrl: string = 'reqform/getapplicantprofileid';
    getAppplicantProfileById() {
      debugger
        var param = { strId: this.loggedUserId };
        var apiUrl = this._getbyApplicantUrl
        this._dataservice.getWithMultipleModel(apiUrl, param)
            .subscribe(response => {
             if(response?.resdata?.userDetails){
                this.res = JSON.parse(response.resdata.userDetails);
                this.applicantdetail=this.res;
             }
                console.log(" this.applicantdetail", this.applicantdetail)
        }, error => {
            console.log(error);
        });
      }




      public responseTag: string = 'listApplication';
      public applicationLists: any = [];
      public _listByPageUrl: string = 'reqform/getbypagesbyid/';
      getListByPage(pageSize) {
        
        debugger
        setTimeout(() => {
          this._pg.getListByPage(1, true, pageSize, '');
            setTimeout(() => {
            }, 300);
        }, 0);
      }
      sendToList(ev) {
        this.applicationLists = ev;
        console.log(" this.applicationLists", this.applicationLists)
      }


 

    public d:any;
  public _getcanDeIdUrl: string = 'reqform/getcandidatedetailsbyid';
  getcandidateDetail(modelEvnt) {
      debugger;
      console.log("modelEvnt",modelEvnt)
      var param = { strId: modelEvnt };
      var apiUrl = this._getcanDeIdUrl
      this._dataservice.getWithMultipleModel(apiUrl, param)
          .subscribe(response => {
              this.res = response;
    
              console.log("details data is ",  this.res)
              this.masterList=JSON.parse(this.res.resdata.regApplicantDetails)
              this.masterListDetails=this.masterList[0];
              if(this.res.resdata.accQlfDetail){
                this.accQlfList=JSON.parse(this.res.resdata.accQlfDetail)
              }
              if(this.res.resdata.wrkExperience){
                this.wrkExpList=JSON.parse(this.res.resdata.wrkExperience)
              }
              if(this.res.resdata.profCertificate){
                this.proCirtificateList=JSON.parse(this.res.resdata.profCertificate)
              }
              console.log("Detalis master ", this.masterList)
              console.log("Detalis accQlfList", this.accQlfList)
              console.log("Detalis wrkExpList", this.wrkExpList)
              console.log("Detalis proCirtificateList", this.proCirtificateList)
             
              this.loadImages( this.masterListDetails.imageNo)
              this.loadImages( this.masterListDetails.CvNo)
             
          }, error => {
              console.log(error);
          });
  }

  public _imgUrl: string = 'reqform/getImage';
  
  loadImages(imgPath: string) {
    debugger
    this.imageUrls = [];
    this._dataservice.getImage(imgPath, this._imgUrl).subscribe(
        (blob) => {
            const url = window.URL.createObjectURL(blob);
             console.error('Failed to load url url:', url);
            this.imageUrls.push(this.sanitizer.bypassSecurityTrustUrl(url));
        },
        (error) => {
            console.error('Failed to load image:', error);
        }
    );
}

  loadSignature(imgPath: string) {
    debugger
    this.signatureUrls = [];
    this._dataservice.getImage(imgPath, this._imgUrl).subscribe(
        (blob) => {
            const url = window.URL.createObjectURL(blob);
            this.signatureUrls.push(this.sanitizer.bypassSecurityTrustUrl(url));
        },
        (error) => {
            console.error('Failed to load image:', error);
        }
    );
}


//download file
//DOWNLOAD FILE FROM PATH
public _fileUrl:string='reqform/getImage'
 downloadFile() {
  debugger
  if(!this.downloadCvPath){
 this.toastr.error("No file Found")
        console.error('No file content received.');
        return;
  }
      this._dataservice.downloadFile(this._fileUrl,this.downloadCvPath).subscribe(response => {
      const blob = response.body;
  if (!blob) {
        this.toastr.error("No file Found")
        console.error('No file content received.');
        return;
      }
    

      const contentDisposition = response.headers.get('Content-Disposition');
      let fileName ="CV";// name+"-"+jobTitle+".pdf";

      if (contentDisposition) {
        const matches = /filename="?([^"]+)"?/.exec(contentDisposition);
        if (matches?.[1]) {
          fileName = matches[1];
        }
      }

      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = fileName;
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
      window.URL.revokeObjectURL(url);
    }, error => {
      console.error('Download failed:', error);
    });
  }

  //DOWNLOAD NID
  //DOWNLOAD FILE FROM PATH
 downloadNID() {
  debugger
  if(!this.downloadNidPath){
 this.toastr.error("No file Found")
        console.error('No file content received.');
        return;
  }
      this._dataservice.downloadFile(this._fileUrl,this.downloadNidPath).subscribe(response => {
      const blob = response.body;
  if (!blob) {
        this.toastr.error("No file Found")
        console.error('No file content received.');
        return;
      }
    

      const contentDisposition = response.headers.get('Content-Disposition');
      let fileName ="NID";// name+"-"+jobTitle+".pdf";

      if (contentDisposition) {
        const matches = /filename="?([^"]+)"?/.exec(contentDisposition);
        if (matches?.[1]) {
          fileName = matches[1];
        }
      }

      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = fileName;
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
      window.URL.revokeObjectURL(url);
    }, error => {
      console.error('Download failed:', error);
    });
  }

  //DOWNLOAD TIN
  //DOWNLOAD FILE FROM PATH
 downloadTIN() {
  debugger
  if(!this.downloadTinPath){
 this.toastr.error("No file Found")
        console.error('No file content received.');
        return;
  }
      this._dataservice.downloadFile(this._fileUrl,this.downloadTinPath).subscribe(response => {
      const blob = response.body;
  if (!blob) {
        this.toastr.error("No file Found")
        console.error('No file content received.');
        return;
      }
    

      const contentDisposition = response.headers.get('Content-Disposition');
      let fileName ="CV";// name+"-"+jobTitle+".pdf";

      if (contentDisposition) {
        const matches = /filename="?([^"]+)"?/.exec(contentDisposition);
        if (matches?.[1]) {
          fileName = matches[1];
        }
      }

      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = fileName;
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
      window.URL.revokeObjectURL(url);
    }, error => {
      console.error('Download failed:', error);
    });
  }




//EDIT UPDATE DATA
public arifMstr:any
public jobSkill:any;
public _geteditUrl: string = 'reqform/getcandidatedetailsbyid';
edit(modelEvnt) {
  this.isEdit=true;
  this.alApply=true
  debugger
  //  this.masterDiv=modelEvnt.masterDiv;
  //  this.applyForm=modelEvnt.applyForm;

   //this.masterDiv=false;
   //this.applyForm=true;
   console.log("master dibvvvvvvvvvvvvvvvvvv is ",modelEvnt)
    debugger;
    console.log("modelEvnt",modelEvnt)
    
   // var param = { strId: modelEvnt.oid };
      var param = { strId: modelEvnt.model.oid };
    var apiUrl = this._geteditUrl
    this._dataservice.getWithMultipleModel(apiUrl, param)
        .subscribe(response => {
            this.res = response;
           console.log(" this.res   this.res   this.res", this.res)
             if(this.res.resdata.regApplicantMaster){
               var mstrData=JSON.parse(this.res.resdata.regApplicantMaster)
               var msrFrm=mstrData[0];
               this.imageId=msrFrm.imageNo;
               this.signatureId=msrFrm.signatureNo;
               this.cvId=msrFrm.CvNo;
               this.tinId=msrFrm.tinNo;
               this.NIDId=msrFrm.NidNo;
                console.log(" this.Edit master File", msrFrm)
               this.updateOidEdit=msrFrm.candidateOid
              
                console.log("this resr posrt ssssssssss", msrFrm)
            this.loadImages( msrFrm.imageNo)
            this.loadSignature( msrFrm.signatureNo)
             this.downloadCvPath=msrFrm.CvNo;
             this.downloadNidPath=msrFrm.NidNo
             this.downloadTinPath=msrFrm.tinNo
             }
              if(this.res.resdata.accQlfDetail){
              this.accQlfList=JSON.parse(this.res.resdata.accQlfDetail)
               this.updateacademicQualifications( this.accQlfList)
            }
       
           
            if(this.res.resdata.wrkExperience){
              this.wrkExpList=JSON.parse(this.res.resdata.wrkExperience)
             this.updateWorkExperiece(this.wrkExpList)
                console.log("this.wrkExpList Is ",this.wrkExpList)
            }
             
         if(this.res.resdata.profCertificate){
              this.proCirtificateList=JSON.parse(this.res.resdata.profCertificate)
              this.updateProfCirtificate(this.proCirtificateList)
               console.log("this.proCirtificateList Is ",this.proCirtificateList)
         }


            console.log("master result is ",msrFrm)
            this.getDistrictById(msrFrm.pre_add_division)
            this.getParDistrictById(msrFrm.par_add_division)
           this.requirementForm.setValue({
              applicantId: msrFrm.candidateOid,
              jobPostId: msrFrm.jobid,
              jobTitle: msrFrm.job_title,
              company: msrFrm.company_name,
              department: msrFrm.department,
              appliedPost: msrFrm.post,
              mobileNumber: msrFrm.mobile_number,
              //campaign: new FormControl(null, Validators.required),
              name: msrFrm.name,
              fatherName: msrFrm.father_name,
              motherName: msrFrm.mother_name,
              nidN: msrFrm.nid,
              tin: msrFrm.tin,
              dateOfBirth:this.getNameToNumDate(msrFrm.date_of_birth),
              birthPlace: msrFrm.birth_place,
              relegion: msrFrm.religion,
              bloodGroup: msrFrm.blood_group,
              gender: msrFrm.gender,
              maritialStatus: msrFrm.marital_status,
              spouseName: msrFrm.spouse_name,
              email: msrFrm.email,
            
              isActive: null,
              preAddDivision: msrFrm.pre_add_division,
              preAddDistrict: msrFrm.per_add_district,
              preAddThana: msrFrm.pre_add_thana,
              preAddPostOffice: msrFrm.pre_add_post_office,
              preAddVillage: msrFrm.pre_add_village,
              preAddDetai: msrFrm.pre_add_detail,

              parAddDivision: msrFrm.par_add_division,
              parAddDistrict: msrFrm.per_add_district,
              parAddThana: msrFrm.par_add_thana,
              parAddPostOffice: msrFrm.par_add_post_office,
              parAddVillage: msrFrm.par_add_village,
              parAddDetai: msrFrm.par_add_detail,

              expectedSelery: msrFrm.expected_selery,
              appliedBy: msrFrm.applied_by,

              imagePath: msrFrm.imageNo,
              signaturePath:msrFrm.signatureNo,
              cvPath: msrFrm.CvNo,
              nidPath: msrFrm.NidNo,
              tinPath: msrFrm.tinNo,
              academicQualifications: this.formBuilder.array([]),
              workExperiences: this.formBuilder.array([]), 
              professionalCirtificate: this.formBuilder.array([]),

           })
           
         
           
        }, error => {
            console.log(error);
        });

      
}



            updateacademicQualifications(acQllf: any[]) {
            this.clearacademicQualifications();
            debugger;
            acQllf.forEach(qlf => {
              var academicQlfGroup = this.formBuilder.group({
                acQlfId: qlf.accQlfOid,
                degree: qlf.degree,
                board: qlf.board,
                institution: qlf.institution,
                major: qlf.major,
                result: qlf.result,
                passingyear: qlf.passingyear,
                applicantId: qlf.applicant_oid,
              });
              this.academicQualifications.push(academicQlfGroup);
              console.log("this.academicQualifications For Update",this.academicQualifications)
            });
            }
            clearacademicQualifications() {
            while (this.academicQualifications.length !== 0) {
              this.academicQualifications.removeAt(0);
            }
            }


            updateWorkExperiece(wrkExp: any[]) {
            this.clearWrkExp();
            debugger;
            wrkExp.forEach(exp => {
              var wrkExpGroup = this.formBuilder.group({
                expId: exp.experienceOid,
                applicantId: exp.applicant_oid,
                companyName: exp.company_name,
                companyType: exp.company_type,
                priodFromDate:[this.convertOracleDateToInput(exp.priod_from_date)],// exp.priod_from_date,
                priodToDate:[this.convertOracleDateToInput(exp.priod_to_date)],// exp.priod_to_date,
                jobDescription: exp.job_description,
                department: exp.department,
                designation: exp.designation,
                jobLocation: exp.location,
              });
              this.workExperiences.push(wrkExpGroup);
            });
            }
            clearWrkExp() {
            while (this.workExperiences.length !== 0) {
              this.workExperiences.removeAt(0);
            }
            }

            updateProfCirtificate(prfCirtificate: any[]) {
            this.clearPrfCrtificate();
            debugger;
            prfCirtificate.forEach(cirtificate => {
              var cirtificateGroup = this.formBuilder.group({
                pCirtificateId: cirtificate.ProfCirtificateOid,
                applicantId: cirtificate.applicant_oid,
                courseName: cirtificate.course_name,
                instution: cirtificate.institution,
                duration: cirtificate.duration,
                achievementDate: cirtificate.achievment_date,
              });
              this.professionalCirtificate.push(cirtificateGroup);
            });
            }
            clearPrfCrtificate() {
            while (this.professionalCirtificate.length !== 0) {
              this.professionalCirtificate.removeAt(0);
            }
            }


  
// scrollToHeader() {
//   const header = document.getElementById('headerSection');
//   if (header) {
//     header.scrollIntoView({ behavior: 'smooth', block: 'start' });
//   }
// }



convertOracleDateToInput(dateString: string): string {
  if (!dateString) return '';

  const parts = dateString.split('-'); // ["08", "MAY", "25"]
  const day = +parts[0];
  const month = parts[1].toUpperCase();
  const shortYear = +parts[2];

  const monthMap: any = {
    JAN: 0, FEB: 1, MAR: 2, APR: 3, MAY: 4, JUN: 5,
    JUL: 6, AUG: 7, SEP: 8, OCT: 9, NOV: 10, DEC: 11
  };

  const fullYear = shortYear < 50 ? 2000 + shortYear : 1900 + shortYear;
  const jsDate = new Date(fullYear, monthMap[month], day);

  return formatDate(jsDate, 'yyyy-MM-dd', 'en-US');
}













 


 



 
}