import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SelectModel } from '../../../Models/SelectModel';
import { OwnerModel } from '../../../Models/OwnerModel';
import { ResultModel } from '../../../Models/ResultModel';
import { OwnerService } from '../../../Services/Owner/owner.service';


@Component({
  selector: 'app-owner',
  templateUrl: './owner.component.html',
  styleUrls: ['./owner.component.css']
})
export class OwnerComponent implements OnInit {


  constructor(private formBuilder: FormBuilder, private OwnerService: OwnerService) { }


  file!: File;
  form: FormGroup;
  Action = "Registro";
  Photo: any;
  ListBooks: SelectModel[];
  List: OwnerModel[] = [];

  showModal = false;


  ngOnInit(): void {

    this.ListAllOwner();

    this.form = this.formBuilder.group(
      {
        IdOwner: '',
        Name: '',
        Address: '',
        Photo: '',
        FileName: '',
        ContentType: '',
        Birthday: ''
      }
    );

  }

  onFileSelected(event: { target: any }) {
    this.file = event.target.files[0];
  }

  SaveChanges() {

    if (this.Action == "Registro") {
      this.SaveOwner();
    }

    if (this.Action == "Modificacion") {
      this.UpdateOwner();
    }

  }

  SaveOwner() {

    const formData = new FormData();

    let Fields = this.GetFields();

    if (this.file) {

      formData.append("OwnerId", Fields.IdOwner.toString());
      formData.append("Name", Fields.Name.toString());
      formData.append("Address", Fields.Address.toString());
      formData.append("Photo", Fields.Photo.toString());
      formData.append("FileName", Fields.FileName.toString());
      formData.append("ContentType", Fields.ContentType.toString());
      formData.append("Birthday", Fields.Birthday.toString());

      formData.append("file", this.file);

    }
    else {
      alert("Seleccione una documento valido");
      return false;
    }
     
    this.OwnerService.SaveOwner(formData).subscribe(
      ResultModel => {
        let Resu = ResultModel as ResultModel;
        if (!Resu.HasError) {
          alert(Resu.Messages);
          this.ListAllOwner();
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

  ViewOwner(id: number) {
    this.ShowModal(true, "Modificacion");
    this.GetOwnerByOwnerId(id);
  }

  UpdateOwner() {

    let Fields = this.GetFields();

    this.OwnerService.UpdateOwner(Fields).subscribe(
      ResultModel => {

        let Resu = ResultModel as ResultModel;
        if (!Resu.HasError) {

          alert(Resu.Messages);
          this.ListAllOwner();
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

  ListAllOwner() {

    this.OwnerService.GetAllOwners().subscribe(

      ResultModel => {

        let Resu = ResultModel as ResultModel;

        if (!Resu.HasError) {

          let Array = Resu.Data as OwnerModel[];

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

  GetOwnerByOwnerId(id: number) {

    this.OwnerService.GetOwnerByOwnerId(id).subscribe(

      ResultModel => {
        let Resu = ResultModel as ResultModel;

        if (!Resu.HasError) {
          let Owner = Resu.Data as OwnerModel;
          this.SetFields(Owner);
        }

        console.log(Resu);

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


  DeleteOwner(id: number) {

    let respuesta = confirm("Esta seguro que desea eliminar el Ownere?");

    if (respuesta)
      this.OwnerService.DeleteOwner(id).subscribe(
        ResultModel => {
          let Resu = ResultModel as ResultModel;

          if (!Resu.HasError) {
            this.ListAllOwner();
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

    this.form.controls['IdOwner'].setValue("");
    this.form.controls['Name'].setValue("");
    this.form.controls['Address'].setValue("");
    this.form.controls['Photo'].setValue("");
    this.form.controls['FileName'].setValue("");
    this.form.controls['ContentType'].setValue("");
    this.form.controls['Birthday'].setValue("");

  }

  GetFields() {

    let Field = new OwnerModel();

    Field.IdOwner = this.form.get("IdOwner").value;
    Field.Name = this.form.get("Name").value;
    Field.Address = this.form.get("Address").value;
    Field.Photo = this.form.get("Photo").value;
    Field.FileName = this.form.get("FileName").value;
    Field.ContentType = this.form.get("ContentType").value;
    //Field.Birthday = this.form.get("Birthday").value;

    const birthdayValue = this.form.get("Birthday")?.value;
    Field.Birthday = birthdayValue ? new Date(birthdayValue).toISOString() : null;

    return Field;

  }

  SetFields(Owner: OwnerModel) {

    this.form.controls['IdOwner'].setValue(Owner.IdOwner);
    this.form.controls['Name'].setValue(Owner.Name);
    this.form.controls['Address'].setValue(Owner.Address);
    this.form.controls['Photo'].setValue(Owner.Photo);
    this.form.controls['FileName'].setValue(Owner.FileName);
    this.form.controls['ContentType'].setValue(Owner.ContentType);
    //this.form.controls['Birthday'].setValue(Owner.Birthday);
    this.form.controls['Birthday'].setValue(
      Owner.Birthday ? Owner.Birthday.split('T')[0] : null
    );

    this.Photo = "data:image/jpeg;base64," + Owner.Photo;

  }

}  
