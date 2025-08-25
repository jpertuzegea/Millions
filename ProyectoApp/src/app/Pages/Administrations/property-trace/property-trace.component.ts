import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PropertyTraceModel } from '../../../Models/PropertyTraceModel';
import { ResultModel } from '../../../Models/ResultModel';
import { ActivatedRoute } from '@angular/router';
import { PropertyTraceService } from '../../../Services/PropertyTrace/property-trace.service';
import { SelectModel } from '../../../Models/SelectModel';
import { PropertyService } from '../../../Services/Property/property.service';

@Component({
  selector: 'app-property-trace',
  templateUrl: './property-trace.component.html',
  styleUrls: ['./property-trace.component.css']
})
export class PropertyTraceComponent implements OnInit {


  constructor(private activatedRoute: ActivatedRoute, private formBuilder: FormBuilder, private PropertyTraceService: PropertyTraceService, private propertyService: PropertyService) { }

  CodeInternal!: string;
  IdProperty!: number;

  file!: File;
  form: FormGroup;
  Action = "Registro";
  List: PropertyTraceModel[] = [];

  showModal = false;
  ListPropertys: SelectModel[];

  ngOnInit(): void {
    this.GetSelecPropertys();

    this.activatedRoute.params.subscribe(Params => {
      this.IdProperty = Params['IdProperty'];
      this.CodeInternal = Params['CodeInternal'];
    });


    this.ListAllPropertyTraceByPropertyId(this.IdProperty);

    this.form = this.formBuilder.group(
      {
        IdPropertyTrace: '',
        DateSale: '',
        Name: '',
        Value: '',
        Tax: '',
        IdProperty: '',
        IdPropertyName: ''
      }
    );

  }


  SaveChanges() {

    if (this.Action == "Registro") {
      this.SavePropertyTrace();
    }

    if (this.Action == "Modificacion") {
      this.UpdatePropertyTrace();
    }

  }

  SavePropertyTrace() {

    let Fields = this.GetFields();

    this.PropertyTraceService.SavePropertyTrace(Fields).subscribe(
      ResultModel => {
        let Resu = ResultModel as ResultModel;
        if (!Resu.HasError) {
          alert(Resu.Messages);
          this.ListAllPropertyTraceByPropertyId(this.IdProperty);
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

  ViewPropertyTrace(id: number) {
    this.ShowModal(true, "Modificacion");
    this.GetPropertyTraceByPropertyTraceId(id);
  }

  UpdatePropertyTrace() {

    let Fields = this.GetFields();

    this.PropertyTraceService.UpdatePropertyTrace(Fields).subscribe(
      ResultModel => {

        let Resu = ResultModel as ResultModel;
        if (!Resu.HasError) {

          alert(Resu.Messages);
          this.ListAllPropertyTraceByPropertyId(this.IdProperty);
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

  ListAllPropertyTraceByPropertyId(Id: number) {

    this.PropertyTraceService.GetAllPropertyTracesByIdProperty(Id).subscribe(

      ResultModel => {

        let Resu = ResultModel as ResultModel;

        if (!Resu.HasError) {

          let Array = Resu.Data as PropertyTraceModel[];

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

  GetPropertyTraceByPropertyTraceId(id: number) {

    this.PropertyTraceService.GetPropertyTraceByPropertyTraceId(id).subscribe(

      ResultModel => {
        let Resu = ResultModel as ResultModel;

        if (!Resu.HasError) {
          let PropertyTrace = Resu.Data as PropertyTraceModel;
          this.SetFields(PropertyTrace);
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


  DeletePropertyTrace(id: number) {

    let respuesta = confirm("Esta seguro que desea eliminar el PropertyTracee?");

    if (respuesta)
      this.PropertyTraceService.DeletePropertyTrace(id).subscribe(
        ResultModel => {
          let Resu = ResultModel as ResultModel;

          if (!Resu.HasError) {
            this.ListAllPropertyTraceByPropertyId(this.IdProperty);
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

    this.form.controls['IdPropertyTrace'].setValue("");
    this.form.controls['DateSale'].setValue("");
    this.form.controls['Name'].setValue("");
    this.form.controls['Value'].setValue("");
    this.form.controls['Tax'].setValue("");
    this.form.controls['IdProperty'].setValue("");
    this.form.controls['IdPropertyName'].setValue("");

  }

  GetFields() {

    let Field = new PropertyTraceModel();

    Field.IdPropertyTrace = this.form.get("IdPropertyTrace").value;
    Field.DateSale = this.form.get("DateSale").value;
    Field.Name = this.form.get("Name").value;
    Field.Value = this.form.get("Value").value;
    Field.Tax = this.form.get("Tax").value;
    Field.IdProperty = this.form.get("IdProperty").value;
    Field.IdPropertyName = this.form.get("IdPropertyName").value;

    return Field;

  }

  SetFields(PropertyTrace: PropertyTraceModel) {

    this.form.controls['IdPropertyTrace'].setValue(PropertyTrace.IdPropertyTrace);
    this.form.controls['DateSale'].setValue(
      new Date(PropertyTrace.DateSale).toISOString().split('T')[0]
    );

    this.form.controls['Name'].setValue(PropertyTrace.Name);
    this.form.controls['Value'].setValue(PropertyTrace.Value);
    this.form.controls['Tax'].setValue(PropertyTrace.Tax);
    this.form.controls['IdProperty'].setValue(PropertyTrace.IdProperty);
    this.form.controls['IdPropertyName'].setValue(PropertyTrace.IdPropertyName);

  }



  public GetSelecPropertys() {
    this.propertyService.GetAllPropertys().subscribe(
      ResultModel => {
        let Resu = ResultModel as unknown as ResultModel;

        if (!Resu.HasError) {

          this.ListPropertys = Resu.Data.map(s => {
            return {
              Value: s.IdProperty,
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

}  
