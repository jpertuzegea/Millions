import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SearchModel } from '../../../Models/SearchModel';
import { PropertyService } from '../../../Services/Property/property.service';
import { ResultModel } from '../../../Models/ResultModel';
import { PropertyModel } from '../../../Models/PropertyModel';
import { PropertyImageService } from '../../../Services/PropertyImage/property-image.service';
import { PropertyImageModel } from '../../../Models/PropertyImageModel';

@Component({
  selector: 'app-galery',
  templateUrl: './galery.component.html',
  styleUrls: ['./galery.component.css']
})
export class GaleryComponent implements OnInit {

  showModal = false;
  PropertyId;

  constructor(private formBuilder: FormBuilder, private propertyService: PropertyService, private propertyImaService: PropertyImageService) { }

  List: PropertyModel[] = [];
  ListPropertyImage: PropertyImageModel[] = [];

  form: FormGroup;

  ngOnInit(): void {
    this.form = this.formBuilder.group(
      {
        Name: '',
        Address: '',
        PriceMin: '',
        PriceMax: ''
      }
    );
  }

  ShowModal(View: boolean, PropertyId: number) {
     this.showModal = View;
    this.PropertyId = PropertyId;
    this.ViewGalery(this.PropertyId);
  }

  Search() {

    let Fields = this.GetFields();
    this.propertyService.SearchPropertys(Fields).subscribe(
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


  CleanFields() {

    this.form.controls['Name'].setValue("");
    this.form.controls['Address'].setValue("");
    this.form.controls['PriceMin'].setValue("");
    this.form.controls['PriceMax'].setValue("");

  }

  GetFields() {

    let Field = new SearchModel();

    Field.Name = this.form.get("Name").value;
    Field.Address = this.form.get("Address").value;
    Field.PriceMin = this.form.get("PriceMin").value;
    Field.PriceMax = this.form.get("PriceMax").value;
    return Field;

  }

  SetFields(searchModel: SearchModel) {

    this.form.controls['Name'].setValue(searchModel.Name);
    this.form.controls['Address'].setValue(searchModel.Address);
    this.form.controls['PriceMin'].setValue(searchModel.PriceMin);
    this.form.controls['PriceMax'].setValue(searchModel.PriceMax);

  }


  ViewGalery(PropertyId: number) {
    this.propertyImaService.GetAllPropertyImagesByIdProperty(PropertyId).subscribe(
      ResultModel => {

        let Resu = ResultModel as ResultModel;

        if (!Resu.HasError) {

          let Array = Resu.Data as PropertyImageModel[];

          if (Resu.Data) {
            this.ListPropertyImage = Array; 
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



}
