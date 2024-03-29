import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { UserDto, UsersAdminService } from '../../services/users/users.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatChipsModule } from '@angular/material/chips';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { ResultModalComponent } from '../result-modal/result-modal.component';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-users-management',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatListModule,
    MatDialogModule,
    MatTableModule,
    MatSortModule,
    MatChipsModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatCheckboxModule,
    MatProgressBarModule,
    MatSnackBarModule],
  templateUrl: './users-management.component.html',
  styleUrls: ['./users-management.component.scss']
})
export class UsersManagementComponent implements OnInit {
  @ViewChild(MatSort, { static: true }) sort!: MatSort;

  addUserForm!: FormGroup;
  displayedColumns: string[] = ['select', 'name', 'email', 'credits', 'delete'];
  dataSource: MatTableDataSource<UserDto> = new MatTableDataSource();
  selection = new SelectionModel<UserDto>(false, []);

  isLoading: boolean = false;
  readonly isEditMode = signal(false);

  constructor(
    private fb: FormBuilder,
    private cd: ChangeDetectorRef,
    public dialog: MatDialog,
    private usersAdminService: UsersAdminService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.createForm();
    this.fetchData();
  }

  fetchData(): void {
    this.usersAdminService.getAllUsers().subscribe(
      results => {
        this.dataSource = new MatTableDataSource(results);
        this.dataSource.sortingDataAccessor = (item: UserDto, property: string) => {
          switch (property) {
            case 'nombre': return item.name;
            case 'email': return item.email;
            case 'credits': return item.credits;
            default: return '';
          }
        };
        this.dataSource.sort = this.sort;

        this.cd.markForCheck();
      }
    );
  }

  onDelete(user: UserDto): void {
    let tagNombre = `Name`;
    let tagEmail = `Email`;
    let title = `Are you sure you want to delete the user?`;
    let description = `${title}\n\n${tagNombre}: ${user.name}\n${tagEmail}: ${user.email}`
    if (confirm(description)) {
      this.isLoading = true;
      this.usersAdminService.deleteUser(user.id).subscribe({
        next: () => {
          this.fetchData();
          this.isLoading = false;
          this.cd.markForCheck();
        },
        error: (error) => {
          this.fetchData();
          this.isLoading = false;
          this.cd.markForCheck();
          this.snackBar.open(`Error while deleting the user.`, `Close`, {
            duration: 5000,
          });
        }
      });
    }
  }

  public turnOffandTurnOn(row: any): void {
    this.selection.toggle(row);
    this.setEditMode(this.selection.selected.length > 0);
    if (this.selection.selected.length > 0) {
      this.addUserForm.patchValue({
        id: this.selection.selected[0].id,
        name: this.selection.selected[0].name,
        email: this.selection.selected[0].email,
        credits: this.selection.selected[0].credits
      });
    }
  }

  public setEditMode(b: boolean): void {
    this.isEditMode.set(b);
    this.addUserForm.get('email')?.clearValidators();
    if (b) {
      this.addUserForm.get('email')?.setValidators([Validators.email, Validators.required, Validators.maxLength(200)]);
    } else {
      this.addUserForm.get('email')?.setValidators([Validators.email, Validators.required, Validators.maxLength(200), this.emailAlreadyExistsValidator.bind(this)]);
    }
    this.addUserForm.get('email')?.updateValueAndValidity();
  }



  createForm(): void {
    this.addUserForm = this.fb.group({
      id: [''],
      email: ['', [Validators.email, Validators.required, Validators.maxLength(200), this.emailAlreadyExistsValidator.bind(this)]],
      name: ['', [Validators.required, Validators.maxLength(200)]],
      credits: [0, [Validators.required]],
    });
  }

  onSubmitUser(): void {
    if (this.addUserForm.invalid || !(this.addUserForm.value.email)) {
      return;
    }
    // Open the modal with the spinner.
    const dialogRef = this.dialog.open(ResultModalComponent, {
      data: { icon: 'autorenew', message: this.isEditMode() ? `Editing user...` : `Inviting user...`, closeButtonText: '', onClose: () => { }, loading: true }
    });

    if (this.isEditMode()) {
      this.usersAdminService.updateUser(
        this.addUserForm.get('id')?.value,{
        name: this.addUserForm.value.name,
        email: this.addUserForm.value.email,
        credits: this.addUserForm.value.credits}).subscribe({
        next: () => {
          this.fetchData();
          this.setEditMode(false);
          this.addUserForm.reset();
          dialogRef.close();
          this.cd.markForCheck();
        },
        error: (error) => {
          this.fetchData();
          dialogRef.close();
          this.cd.markForCheck();
          this.snackBar.open(`Error editing user.`, `Close`, {
            duration: 5000,
          });
        }
      });
    } else {
      this.usersAdminService.createUser({
        name: this.addUserForm.value.name,
        email: this.addUserForm.value.email,
        credits: this.addUserForm.value.credits}).subscribe({
        next: () => {
          this.fetchData();
          this.addUserForm.reset();
          dialogRef.close();
          this.cd.markForCheck();
        },
        error: (error) => {
          this.fetchData();
          dialogRef.close();
          this.cd.markForCheck();
          this.snackBar.open(`Error inviting user.`, `Close`, {
            duration: 5000,
          });
        }
      });
    }
  }

  private emailAlreadyExistsValidator(control: FormControl): { [key: string]: any } | null {
    let email = control.value;
    if (email) {
      if (this.dataSource.data.some(r => r.email === email))
        return { 'emailExists': true };  // This represents a validation error
      else
        return null;  // No error
    }
    return null; // No error
  }
}
