import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule, registerLocaleData } from '@angular/common';

import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

import { MenuComponent } from './Components/menu/menu.component';
import { ContactComponent } from './Components/contact/contact.component';

import { LabelModule } from './Module-Label/label.module';
import { PhotoModule } from './Module-Photo/photo.module';
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { en_US } from 'ng-zorro-antd/i18n';
import en from '@angular/common/locales/en';
import { FormsModule } from '@angular/forms';

registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    ContactComponent
  ],
  imports: [
    LabelModule,
    PhotoModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    CommonModule,
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    FormsModule
  ],
  providers: [{ provide: NZ_I18N, useValue: en_US }],
  bootstrap: [AppComponent]
})
export class AppModule { }
