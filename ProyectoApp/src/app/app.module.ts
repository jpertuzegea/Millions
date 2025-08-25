import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FooterComponent } from './Shared/footer/footer.component';
import { HomeComponent } from './Shared/home/home.component';
import { NavBarComponent } from './Shared/nav-bar/nav-bar.component';
import { WorkSpaceComponent } from './Shared/work-space/work-space.component';
import { LoginComponent } from './Pages/Security/login/login.component';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InterceptorService } from './Services/Interceptors/interceptor.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { OwnerComponent } from './Pages/Administrations/owner/owner.component';
import { PropertyComponent } from './Pages/Administrations/property/property.component';
import { PropertyImageImagesComponent } from './Pages/Administrations/property-images/property-images.component';
import { PropertyTraceComponent } from './Pages/Administrations/property-trace/property-trace.component';
import { GaleryComponent } from './Pages/Administrations/galery/galery.component';
;

@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    HomeComponent,
    NavBarComponent,
    WorkSpaceComponent,
    LoginComponent,
    OwnerComponent,
    PropertyComponent,
    PropertyImageImagesComponent,
    PropertyTraceComponent,
    GaleryComponent,
  ],

  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    CommonModule
  ],

  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: InterceptorService,
      multi: true
    }
  ],

  bootstrap: [AppComponent]

})
export class AppModule { }
