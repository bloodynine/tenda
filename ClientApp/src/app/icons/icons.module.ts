import { NgModule } from '@angular/core';

import { FeatherModule } from 'angular-feather';
import {Square, CheckSquare, PlusSquare, Repeat, Plus, X} from 'angular-feather/icons';

const icons = {
  Square, PlusSquare, CheckSquare, Repeat, Plus, X
};

@NgModule({
  imports: [
    FeatherModule.pick(icons)
  ],
  exports: [
    FeatherModule
  ]
})
export class IconsModule { }
