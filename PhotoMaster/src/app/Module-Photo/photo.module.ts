import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

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
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzImageModule } from 'ng-zorro-antd/image';
import { NzCollapseModule } from 'ng-zorro-antd/collapse';
import { NzAffixModule } from 'ng-zorro-antd/affix';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzCarouselModule } from 'ng-zorro-antd/carousel';
import { NzBackTopModule } from 'ng-zorro-antd/back-top';

import { DetailViewComponent } from './detail-view/detail-view.component';
import { GlobalViewComponent } from './global-view/global-view.component';
import { HomeViewComponent } from './home-view/home-view.component';
import { PhotoListComponent } from './photo-list/photo-list.component';
import { PhotoDetailComponent } from './photo-detail/photo-detail.component';
import { PhotoWallViewComponent } from './photo-wall-view/photo-wall-view.component';

@NgModule({
  declarations: [DetailViewComponent, GlobalViewComponent, HomeViewComponent, PhotoListComponent, PhotoDetailComponent, PhotoWallViewComponent],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
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
    NzToolTipModule,
    NzDatePickerModule,
    NzSelectModule,
    NzUploadModule,
    NzDropDownModule,
    NzImageModule,
    NzCollapseModule,
    NzAffixModule,
    NzRadioModule,
    NzTagModule,
    NzCarouselModule,
    NzBackTopModule
  ]
})
export class PhotoModule { }
