import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentItemComponent } from './component-item.component';

describe('ComponentItemComponent', () => {
  let component: ComponentItemComponent;
  let fixture: ComponentFixture<ComponentItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ComponentItemComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComponentItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
