import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './Shared/home/home.component';
import { AuthenticationGuard } from './Guards/authentication.guard';
import { OwnerComponent } from './Pages/Administrations/owner/owner.component';
import { PropertyComponent } from './Pages/Administrations/property/property.component'; 
import { PropertyImageImagesComponent } from './Pages/Administrations/property-images/property-images.component'; 
import { PropertyTraceComponent } from './Pages/Administrations/property-trace/property-trace.component';
import { GaleryComponent } from './Pages/Administrations/galery/galery.component';


const routes: Routes = [

  { path: 'home', component: HomeComponent, canActivate: [AuthenticationGuard] },

  { path: 'owner', component: OwnerComponent, canActivate: [AuthenticationGuard] },
  { path: 'property', component: PropertyComponent, canActivate: [AuthenticationGuard] },
  { path: 'propertyImages/:IdProperty/:CodeInternal', component: PropertyImageImagesComponent, canActivate: [AuthenticationGuard] },
  { path: 'PropertyTrace/:IdProperty/:CodeInternal', component: PropertyTraceComponent, canActivate: [AuthenticationGuard] },
  { path: 'galery', component: GaleryComponent, canActivate: [AuthenticationGuard] },

  
  

  //  
  { path: '**', redirectTo: 'home' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
