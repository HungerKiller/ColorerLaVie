import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageModule } from 'ng-zorro-antd/message';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';

import { DetailViewComponent } from '../Module-Photo/Components/detail-view/detail-view.component';
import { GlobalViewComponent } from '../Module-Photo/Components/global-view/global-view.component';
import { HomeViewComponent } from '../Module-Photo/Components/home-view/home-view.component';
import { PhotoComponent } from './photo.component';
import { PhotoListComponent } from './Components/photo-list/photo-list.component';
import { PhotoDetailComponent } from './Components/photo-detail/photo-detail.component';

@NgModule({
  declarations: [DetailViewComponent, GlobalViewComponent, HomeViewComponent, PhotoComponent, PhotoListComponent, PhotoDetailComponent],
  imports: [
    CommonModule,
    RouterModule,
    NzTableModule,
    NzButtonModule,
    NzDividerModule,
    NzPaginationModule,
    NzIconModule,
    NzGridModule,
    NzDrawerModule,
    NzInputModule,
    NzMessageModule,
    NzPopconfirmModule,
    NzToolTipModule
  ]
})
export class PhotoModule { }
