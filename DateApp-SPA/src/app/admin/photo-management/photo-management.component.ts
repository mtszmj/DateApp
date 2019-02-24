import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ModeratorService } from 'src/app/_services/moderator.service';
import { Photo } from 'src/app/_models/photo';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent implements OnInit {
  users: User[];
  photos: any;

  constructor(private moderatorService: ModeratorService) { }

  ngOnInit() {
    this.getPhotosForApproval();
  }

  getUsers() {
    this.moderatorService.getUsersWithUnapprovedPhotos().subscribe(result => {
      this.users = result.result;
    }, error => {
      console.log(error);
    });
  }

  getPhotosForApproval() {
    this.moderatorService.getPhotosForApproval().subscribe((photos) => {
      this.photos = photos;
    }, error => {
      console.log(error);
    });
  }

  approvePhoto(photoId) {
    this.moderatorService.approvePhoto(photoId).subscribe(() => {
      this.photos.splice(this.photos.findIndex(p => p.id === photoId), 1);
    }, error => {
      console.log(error);
    });
  }

  rejectPhoto(photoId) {
    this.moderatorService.rejectPhoto(photoId).subscribe(() => {
      this.photos.splice(this.photos.findIndex(p => p.id === photoId), 1);
    }, error => {
      console.log(error);
    });
  }

}
