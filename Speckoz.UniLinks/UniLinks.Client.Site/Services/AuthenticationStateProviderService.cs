﻿using Blazored.SessionStorage;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using UniLinks.Dependencies.Data.VO.Coordinator;
using UniLinks.Dependencies.Data.VO.Student;

namespace UniLinks.Client.Site.Services
{
	public class AuthenticationStateProviderService : AuthenticationStateProvider
	{
		private readonly IConfiguration _configuration;
		private readonly ISessionStorageService _sessionStorage;
		private readonly NavigationManager _navigation;
		private readonly ThemeService _themeService;

		public AuthenticationStateProviderService(IConfiguration configuration, ISessionStorageService sessionStorage, NavigationManager navigation, ThemeService themeService)
		{
			_configuration = configuration;
			_sessionStorage = sessionStorage;
			_navigation = navigation;
			_themeService = themeService;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			ClaimsPrincipal user;
			try
			{
				user = ValidateToken(await _sessionStorage.GetItemAsync<string>("token"));
			}
			catch
			{
				user = new ClaimsPrincipal();
			}

			return await Task.FromResult(new AuthenticationState(user));
		}

		public ClaimsPrincipal ValidateToken(string jwtToken)
		{
			IdentityModelEventSource.ShowPII = true;

			var validationParameters = new TokenValidationParameters
			{
				ValidateLifetime = true,
				RequireExpirationTime = true,

				ValidAudience = _configuration["JWT:Audience"],
				ValidIssuer = _configuration["JWT:Issuer"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]))
			};

			return new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out _);
		}

		public async Task MarkUserWithAuthenticatedAsync<T>(T user)
		{
			if (user is AuthCoordinatorVO coord)
			{
				await _sessionStorage.SetItemAsync("userId", coord.CoordinatorId);
				await _sessionStorage.SetItemAsync("email", coord.Email);
				await _sessionStorage.SetItemAsync("name", coord.Name);
				await _sessionStorage.SetItemAsync("courseId", coord.CourseId);
				await _sessionStorage.SetItemAsync("token", coord.Token);
				NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(ValidateToken(coord.Token))));
			}
			else if (user is StudentVO student)
			{
				await _sessionStorage.SetItemAsync("userId", student.StudentId);
				await _sessionStorage.SetItemAsync("email", student.Email);
				await _sessionStorage.SetItemAsync("name", student.Name);
				await _sessionStorage.SetItemAsync("courseId", student.CourseId);
				await _sessionStorage.SetItemAsync("disciplines", string.Join(';', student.Disciplines.Select(x => x.DisciplineId.ToString()).ToArray()));
				await _sessionStorage.SetItemAsync("token", student.Token);
				NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(ValidateToken(student.Token))));
			}
		}

		public async Task LogoutUserAsync()
		{
			_navigation.NavigateTo("/");
			await _sessionStorage.ClearAsync();
			await _themeService.ChangeTheme(false);
			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
		}
	}
}