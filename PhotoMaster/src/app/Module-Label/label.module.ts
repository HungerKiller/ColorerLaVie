import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';

import { LabelComponent } from './label.component';
import { LabelDetailComponent } from './Components/label-detail/label-detail.component';
import { LabelListComponent } from './Components/label-list/label-list.component';

@NgModule({
  declarations: [LabelComponent, LabelDetailComponent, LabelListComponent],
  imports: [
    CommonModule,
    RouterModule,
    MatPaginatorModule,
    MatTableModule
  ]
})
export class LabelModule { }
