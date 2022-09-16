import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiceActivityPanelComponent } from './service-activity-panel.component';

describe('ServiceActivityPanelComponent', () => {
  let component: ServiceActivityPanelComponent;
  let fixture: ComponentFixture<ServiceActivityPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ServiceActivityPanelComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ServiceActivityPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
