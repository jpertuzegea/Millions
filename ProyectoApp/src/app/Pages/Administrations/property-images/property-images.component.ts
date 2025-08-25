import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PropertyImageModel } from '../../../Models/PropertyImageModel';
import { SelectModel } from '../../../Models/SelectModel';
import { ResultModel } from '../../../Models/ResultModel';
import { PropertyImageService } from '../../../Services/PropertyImage/property-image.service';


@Component({
  selector: 'app-property-images',
  templateUrl: './property-images.component.html',
  styleUrls: ['./property-images.component.css']
})
export class PropertyImageImagesComponent implements OnInit {

  IdPropertyImage!: string;
  CodeInternal!: string;
  IdProperty!: number;
  constructor(private activatedRoute: ActivatedRoute, private formBuilder: FormBuilder, private propertyImageService: PropertyImageService, private router: Router) { }

  form: FormGroup;
  Action = "Registro";
  Photo: any;
  ListOwners: SelectModel[];
  List: PropertyImageModel[] = [];
  file!: File;

  ListYesNo = [
    { Value: true, Text: 'Sí' },
    { Value: false, Text: 'No' }
  ];


  showModal = false;

  ngOnInit(): void {

    this.activatedRoute.params.subscribe(Params => {
      this.IdPropertyImage = Params['IdPropertyImage'];
      this.IdProperty = Params['IdProperty'];
      this.CodeInternal = Params['CodeInternal'];
    });

    this.ListAllPropertyImageByIdProperty(this.IdProperty);

    this.form = this.formBuilder.group(
      {
        IdPropertyImage: '',
        IdProperty: '',
        Photo: '',
        PropertyName: '',
        FileName: '',
        ContentType: '',
        Enabled: ''
      }
    );
  }


  onFileSelected(event: { target: any }) {
    this.file = event.target.files[0];
  }


  SaveChanges() {

    if (this.Action == "Registro") {
      this.SavePropertyImage();
    }

    if (this.Action == "Modificacion") {
      this.UpdatePropertyImage();
    }

  }

  SavePropertyImage() {
    const formData = new FormData();

    let Fields = this.GetFields();

    if (this.file) {

      formData.append("IdPropertyImage", Fields.IdPropertyImage.toString());
      formData.append("IdProperty", this.IdProperty.toString());
      formData.append("Photo", Fields.Photo.toString());
      formData.append("FileName", Fields.FileName.toString());
      formData.append("ContentType", Fields.ContentType.toString());
      formData.append("Enabled", Fields.Enabled.toString());

      formData.append("file", this.file);

    }
    else {
      alert("Seleccione una documento valido");
      return false;
    }

    this.propertyImageService.SavePropertyImage(formData).subscribe(
      ResultModel => {
        let Resu = ResultModel as ResultModel;
        if (!Resu.HasError) {
          alert(Resu.Messages);
          this.ListAllPropertyImageByIdProperty(this.IdProperty);
          this.ShowModal(false, "Registro");
        } else {
          alert(Resu.Messages);
        }
      }, error => {

        if (error.status == 401) {
          alert("No Autorizado");
        } else {
          alert(JSON.stringify(error));
        }

      }
    );

  }

  ViewPropertyImage(id: number) {
    this.ShowModal(true, "Modificacion");
    this.GetPropertyImageByPropertyImageId(id);
  }

  UpdatePropertyImage() {

    let Fields = this.GetFields();

    this.propertyImageService.UpdatePropertyImage(Fields).subscribe(
      ResultModel => {

        let Resu = ResultModel as ResultModel;
        if (!Resu.HasError) {

          alert(Resu.Messages);
          this.ListAllPropertyImageByIdProperty(this.IdProperty);
          this.ShowModal(false, "Registro");

        } else {
          alert(Resu.Messages);
        }
      }, error => {

        if (error.status == 401) {
          alert("No Autorizado");
        } else {
          alert(JSON.stringify(error));
        }

      }
    );
  }

  ListAllPropertyImageByIdProperty(IdProperty: number) {

    this.propertyImageService.GetAllPropertyImagesByIdProperty(IdProperty).subscribe(

      ResultModel => {

        let Resu = ResultModel as ResultModel;

        if (!Resu.HasError) {

          let Array = Resu.Data as PropertyImageModel[];

          if (Resu.Data) {
            this.List = Array;
          } else {
            console.log('sin datos para mostrar')
          }

        } else {
          alert(Resu.Messages);
        }

      }, error => {

        if (error.status == 401) {
          alert("No Autorizado");
        } else {
          alert(JSON.stringify(error));
        }

      }
    );

  }


  GetPropertyImageByPropertyImageId(id: number) {

    this.propertyImageService.GetPropertyImageByPropertyImageId(id).subscribe(

      ResultModel => {
        let Resu = ResultModel as ResultModel;

        if (!Resu.HasError) {
          let PropertyImage = Resu.Data as PropertyImageModel;
          this.SetFields(PropertyImage);
        }
         

      }, error => {

        if (error.status == 401) {
          alert("No Autorizado");
        } else {
          alert(JSON.stringify(error));
        }

      }
    );

  }


  ShowModal(View: boolean, Action: string) {

    if (!View) {
      this.CleanFields();
    }
    this.showModal = View;
    this.Action = Action;
  }


  DeletePropertyImage(id: number) {
     
    let respuesta = confirm("Esta seguro que desea eliminar el PropertyImagee?");

    if (respuesta)
      this.propertyImageService.DeletePropertyImage(id).subscribe(
        ResultModel => {
          let Resu = ResultModel as ResultModel;

          if (!Resu.HasError) {
            this.ListAllPropertyImageByIdProperty(this.IdProperty);
            alert(Resu.Messages);
          } else {
            alert(Resu.Messages);
          }

        }, error => {
          alert(JSON.stringify(error));
        }
      );
  }


  CleanFields() {

    this.form.controls['IdPropertyImage'].setValue("");
    this.form.controls['IdProperty'].setValue("");
    this.form.controls['Photo'].setValue("");
    this.form.controls['PropertyName'].setValue("");
    this.form.controls['FileName'].setValue("");
    this.form.controls['ContentType'].setValue("");
    this.form.controls['Enabled'].setValue("");
  }

  GetFields() {

    let Field = new PropertyImageModel();

    Field.IdPropertyImage = this.form.get("IdPropertyImage").value; 
    Field.PropertyName = this.form.get("PropertyName").value;
    Field.FileName = this.form.get("FileName").value;
    Field.ContentType = this.form.get("ContentType").value;
    Field.Photo = this.form.get("Photo").value;
    Field.Enabled = this.form.get("Enabled").value;

    return Field;

  }

  SetFields(PropertyImage: PropertyImageModel) {
     
    this.form.controls['IdPropertyImage'].setValue(PropertyImage.IdPropertyImage);
    this.form.controls['PropertyName'].setValue(PropertyImage.PropertyName);
    this.form.controls['FileName'].setValue(PropertyImage.FileName);
    this.form.controls['ContentType'].setValue(PropertyImage.ContentType);
    this.form.controls['Enabled'].setValue(PropertyImage.Enabled);

  }
}




