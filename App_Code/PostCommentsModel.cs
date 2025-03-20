using System;
using System.Collections.Generic;

/// <summary>
/// Summary description for GoogleMapsAPI
/// </summary>
namespace GoogleMapsAPI.Places
{
	public class PostCommentsModel
	{
		public Guid PostCommentId { get; set; }
		public string Comment1 { get; set; }
		public DateTime CreatedOn { get; set; }
		public Guid CommentId { get; set; }
		public bool IsEdit { get; set; }
		public bool IsDelete { get; set; }
		public Guid PostId { get; set; }
		public string TimeAgo { get; set; }
		public string Author { get; set; }
		public string ProfileUrl { get; set; }
		public string ImgProfileUrl { get; set; }
		public List<Reply> Replies { get; set; } // Replies as a list of Reply objects
		public string TotalPostComments { get; set; }
	}

	public class Reply
	{
		public Guid PostCommentId { get; set; }
		public Guid CommentId { get; set; }
		public string Comment1 { get; set; }
		public DateTime CreatedOn { get; set; }
		public string TimeAgo { get; set; }
		public bool IsEdit { get; set; }
		public bool IsDelete { get; set; }
		public Guid PostId { get; set; }
		public Guid ParentCommentId { get; set; }
		public string ProfileUrl { get; set; }
		public string Author { get; set; }
		public string ImgProfileUrl { get; set; }
	}
}