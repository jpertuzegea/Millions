import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PropertyModel } from '../../../Models/PropertyModel';
import { SelectModel } from '../../../Models/SelectModel';
import { PropertyService } from '../../../Services/Property/property.service';
import { ResultModel } from '../../../Models/ResultModel';
import { OwnerService } from '../../../Services/Owner/owner.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-property',
  templateUrl: './property.component.html',
  styleUrls: ['./property.component.css']
})
export class PropertyComponent implements OnInit {
  constructor(private formBuilder: FormBuilder, private PropertyService: PropertyService, private ownerService: OwnerService, private router: Router) { }

  form: FormGroup;
  Action = "Registro";
  Photo: any;
  ListOwners: SelectModel[];
  List: PropertyModel[] = [];

  showModal = false;

  ngOnInit(): void {

    this.GetSelecOwner();

    this.ListAllProperty();

    this.form = this.formBuilder.group(
      {
        IdProperty: '',
        Name: '',
        Address: '',
        Price: '',
        CodeInternal: '',
        Year: '',
        IdOwner: '',
        OwnerName: ''
      }
    );
  }


  SaveChanges() {

    if (this.Action == "Registro") {
      this.SaveProperty();
    }

    if (this.Action == "Modificacion") {
      this.UpdateProperty();
    }

  }

  SaveProperty() {

    let Fields = this.GetFields();

    this.PropertyService.SaveProperty(Fields).subscribe(
      ResultModel => {
        let Resu = ResultModel as ResultModel;
        if (!Resu.HasError) {
          alert(Resu.Messages);
          this.ListAllProperty();
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

  ViewProperty(id: number) {
    this.ShowModal(true, "Modificacion");
    this.GetPropertyByPropertyId(id);
  }

  UpdateProperty() {

    let Fields = this.GetFields();

    this.PropertyService.UpdateProperty(Fields).subscribe(
      ResultModel => {

        let Resu = ResultModel as ResultModel;
        if (!Resu.HasError) {

          alert(Resu.Messages);
          this.ListAllProperty();
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

  ListAllProperty() {

    this.PropertyService.GetAllPropertys().subscribe(

      ResultModel => {

        let Resu = ResultModel as ResultModel;

        if (!Resu.HasError) {

          let Array = Resu.Data as PropertyModel[];

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

  GetPropertyByPropertyId(id: number) {

    this.PropertyService.GetPropertyByPropertyId(id).subscribe(

      ResultModel => {
        let Resu = ResultModel as ResultModel;

        if (!Resu.HasError) {
          let Property = Resu.Data as PropertyModel;
          this.SetFields(Property);
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


  DeleteProperty(id: number) {

    let respuesta = confirm("Esta seguro que desea eliminar el Propertye?");

    if (respuesta)
      this.PropertyService.DeleteProperty(id).subscribe(
        ResultModel => {
          let Resu = ResultModel as ResultModel;

          if (!Resu.HasError) {
            this.ListAllProperty();
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

    this.form.controls['IdProperty'].setValue("");
    this.form.controls['Name'].setValue("");
    this.form.controls['Address'].setValue("");
    this.form.controls['Price'].setValue("");
    this.form.controls['CodeInternal'].setValue("");
    this.form.controls['Year'].setValue("");
    this.form.controls['IdOwner'].setValue("");
    this.form.controls['OwnerName'].setValue("");

  }

  GetFields() {

    let Field = new PropertyModel();

    Field.IdProperty = this.form.get("IdProperty").value;
    Field.Name = this.form.get("Name").value;
    Field.Address = this.form.get("Address").value;
    Field.Price = this.form.get("Price").value;
    Field.CodeInternal = this.form.get("CodeInternal").value;
    Field.Year = this.form.get("Year").value;
    Field.IdOwner = this.form.get("IdOwner").value;
    Field.OwnerName = this.form.get("OwnerName").value;

    return Field;

  }

  SetFields(Property: PropertyModel) {

    this.form.controls['IdProperty'].setValue(Property.IdProperty);
    this.form.controls['Name'].setValue(Property.Name);
    this.form.controls['Address'].setValue(Property.Address);
    this.form.controls['Price'].setValue(Property.Price);
    this.form.controls['CodeInternal'].setValue(Property.CodeInternal);
    this.form.controls['Year'].setValue(Property.Year);
    this.form.controls['IdOwner'].setValue(Property.IdOwner);
    this.form.controls['OwnerName'].setValue(Property.OwnerName);

  }

  public GetSelecOwner() {
    this.ownerService.GetAllOwners().subscribe(
      ResultModel => {
        let Resu = ResultModel as unknown as ResultModel;

        if (!Resu.HasError) {

          this.ListOwners = Resu.Data.map(s => {
            return {
              Value: s.IdOwner,
              Text: `${s.Name}`
            };
          });

        }
      }, error => {
        console.log(error);
        if (error.status == 401) {
          alert("No Autorizado");
        } else {
          alert(JSON.stringify(error));
        }
      }
    );
  }

  ViewPropertyImages(IdProperty: number, CodeInternal: string) {
    this.router.navigate(['propertyImages', IdProperty, CodeInternal]);
  }


  ViewPropertyTrace(IdProperty: number, CodeInternal: string) {
    this.router.navigate(['PropertyTrace', IdProperty, CodeInternal]);
  }

  
}
