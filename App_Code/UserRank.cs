using System;
using System.Linq;

namespace Stability
{
	public class UserRank
	{
		public Guid OrganizationUserRankingId { get; set; }
		public Guid OrganizationId { get; set; }
		public Guid UserId { get; set; }
		public decimal TotalPoints { get; set; }
		public int? RankPosition { get; set; }
		public decimal VolunteerHours { get; set; }
		public decimal VolunteerHoursScore { get; set; }
		public int ClaimedPositionsCount { get; set; }
		public decimal ClaimedPositionsScore { get; set; }
		public int SkillsCount { get; set; }
		public decimal SkillsScore { get; set; }
		public int ResourcesCount { get; set; }
		public decimal ResourcesScore { get; set; }
		public int ProfileCompletenessCount { get; set; }
		public decimal ProfileCompletenessScore { get; set; }
		public bool HasPhoto { get; set; }
		public bool HasDescription { get; set; }
		public bool HasExperience { get; set; }
		public bool HasAddress { get; set; }
		public bool HasPhone { get; set; }
		public string FormulaVersion { get; set; }
		public DateTime CalculationDate { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedOn { get; set; }
		public Guid? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public Guid? ModifiedBy { get; set; }
		public int? LowestRankPosition { get; set; }

		public static UserRank Load(Guid userId, Guid? organizationId = null)
		{
			using (var dc = new CrowdReliefDBDataContext())
			{
				var ranking = (from r in dc.OrganizationUserRankings
							   join m in dc.aspnet_Memberships on r.UserId equals m.UserId
							   where m.IsApproved &&
									 r.UserId == userId &&
									 (organizationId == null || r.OrganizationId == organizationId)
							   orderby r.CalculationDate descending
							   select r).FirstOrDefault();

				if (ranking == null) return null;

				// Determine total number of ranks (i.e., highest position value)
				var lowestRank = (from r in dc.OrganizationUserRankings
								  join m in dc.aspnet_Memberships on r.UserId equals m.UserId
								  where m.IsApproved &&
										r.IsActive &&
										(organizationId == null || r.OrganizationId == organizationId)
								  select r.RankPosition).Max();

				return new UserRank
				{
					OrganizationUserRankingId = ranking.OrganizationUserRankingId,
					OrganizationId = ranking.OrganizationId,
					UserId = ranking.UserId,
					TotalPoints = ranking.TotalPoints,
					RankPosition = ranking.RankPosition,
					VolunteerHours = ranking.VolunteerHours,
					VolunteerHoursScore = ranking.VolunteerHoursScore,
					ClaimedPositionsCount = ranking.ClaimedPositionsCount,
					ClaimedPositionsScore = ranking.ClaimedPositionsScore,
					SkillsCount = ranking.SkillsCount,
					SkillsScore = ranking.SkillsScore,
					ResourcesCount = ranking.ResourcesCount,
					ResourcesScore = ranking.ResourcesScore,
					ProfileCompletenessCount = ranking.ProfileCompletenessCount,
					ProfileCompletenessScore = ranking.ProfileCompletenessScore,
					HasPhoto = ranking.HasPhoto,
					HasDescription = ranking.HasDescription,
					HasExperience = ranking.HasExperience,
					HasAddress = ranking.HasAddress,
					HasPhone = ranking.HasPhone,
					FormulaVersion = ranking.FormulaVersion,
					CalculationDate = ranking.CalculationDate,
					IsActive = ranking.IsActive,
					CreatedOn = ranking.CreatedOn,
					CreatedBy = ranking.CreatedBy,
					ModifiedOn = ranking.ModifiedOn,
					ModifiedBy = ranking.ModifiedBy,
					LowestRankPosition = lowestRank
				};
			}
		}

		public string GetRankSummary()
		{
			if (RankPosition == 1)
				return "🏆 Currently ranked #1 — leading the entire community!";

			if (LowestRankPosition <= 1)
				return "The only ranked user so far.";

			double? percentile = (double)RankPosition / LowestRankPosition;

			if (percentile <= 0.01)
				return string.Format("🔥 In the top 1%! (Rank #{0} of {1})", RankPosition, LowestRankPosition);
			else if (percentile <= 0.05)
				return string.Format("🌟 In the top 5%! (Rank #{0} of {1})", RankPosition, LowestRankPosition);
			else if (percentile <= 0.10)
				return string.Format("👏 In the top 10%! Keep going — Rank #{0}", RankPosition);
			else if (percentile <= 0.25)
				return string.Format("👍 Doing great — in the top 25% (Rank #{0})", RankPosition);
			else if (percentile <= 0.50)
				return string.Format("🔥 In the top half — Rank #{0} of {1}", RankPosition, LowestRankPosition);
			else if (percentile <= 0.75)
				return string.Format("👍 Making progress — Rank #{0} of {1}", RankPosition, LowestRankPosition);
			else
				return string.Format("👍 Just getting started — Rank #{0} of {1}. Every bit counts!", RankPosition, LowestRankPosition);
		}

		public string GetRankTooltip()
		{
			var tips = new[]
			{
				"Log more volunteer hours — every hour counts (capped fairly).\n",
				"Sign up for event roles or tasks when available.\n",
				"Add more skills to your profile.\n",
				"Share resources you’re willing to lend or donate.\n",
				"Complete your profile: add a photo, description, phone number, and address.\n",
				"Stay active — regular contributors move up faster."
			};

			var random = new Random();
			var tip = tips[random.Next(tips.Length)];

			return string.Format(
				"Rank is based on your activity and how complete your profile is.\n\n " +
				"Earn points for:\n" +
				"• Volunteer hours\n" +
				"• Volunteer roles claimed\n" +
				"• Skills and resources listed\n" +
				"• A complete profile (photo, phone, etc.)\n\n" +
				"🟢 Tip to improve your rank:\n• {0}", tip);
		}

		public string GetVolunteerValue()
		{
			const decimal hourlyRate = 31.00m;
			decimal value = Math.Round((decimal)VolunteerHours * hourlyRate, 2);
			return string.Format("{0:C}", value); // Formats as $X,XXX.XX based on system locale
		}

		public class NeighborRank
		{
			public Guid? AboveUserId { get; set; }
			public string AboveNameWithRank { get; set; }

			public Guid? BelowUserId { get; set; }
			public string BelowNameWithRank { get; set; }
		}
		public NeighborRank GetNeighborRanks()
		{
			using (var dc = new CrowdReliefDBDataContext())
			{
				// Count only users in the org whose Membership is approved
				var totalUsers = (from r in dc.OrganizationUserRankings
								  join u in dc.aspnet_Memberships on r.UserId equals u.UserId
								  where r.OrganizationId == this.OrganizationId
										&& u.IsApproved == true
								  select r).Count();

				if (totalUsers <= 1)
				{
					return new NeighborRank
					{
						AboveUserId = null,
						AboveNameWithRank = null,
						BelowUserId = null,
						BelowNameWithRank = null
					};
				}

				var aboveUser = this.RankPosition > 1
					? (from r in dc.OrganizationUserRankings
					   join p in dc.Profiles on r.UserId equals p.UserId
					   join u in dc.aspnet_Memberships on r.UserId equals u.UserId
					   where r.OrganizationId == this.OrganizationId &&
							 r.RankPosition == this.RankPosition - 1 &&
							 u.IsApproved == true
					   select new { r.UserId, r.RankPosition, p.Firstname, p.Lastname })
					  .FirstOrDefault()
					: null;

				var belowUser = this.RankPosition < totalUsers
					? (from r in dc.OrganizationUserRankings
					   join p in dc.Profiles on r.UserId equals p.UserId
					   join u in dc.aspnet_Memberships on r.UserId equals u.UserId
					   where r.OrganizationId == this.OrganizationId &&
							 r.RankPosition == this.RankPosition + 1 &&
							 u.IsApproved == true
					   select new { r.UserId, r.RankPosition, p.Firstname, p.Lastname })
					  .FirstOrDefault()
					: null;

				var neighbor = new NeighborRank();

				if (aboveUser != null)
				{
					neighbor.AboveUserId = aboveUser.UserId;
					neighbor.AboveNameWithRank = string.Format("{0} {1} (Rank #{2})",
						aboveUser.Firstname, aboveUser.Lastname, aboveUser.RankPosition);
				}

				if (belowUser != null)
				{
					neighbor.BelowUserId = belowUser.UserId;
					neighbor.BelowNameWithRank = string.Format("{0} {1} (Rank #{2})",
						belowUser.Firstname, belowUser.Lastname, belowUser.RankPosition);
				}


				return neighbor;
			}
		}

	}
}