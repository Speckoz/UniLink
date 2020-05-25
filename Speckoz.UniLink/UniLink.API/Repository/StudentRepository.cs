﻿using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UniLink.API.Data;
using UniLink.API.Repository.Interfaces;
using UniLink.Dependencies.Models;

namespace UniLink.API.Repository
{
	public class StudentRepository : BaseRepository, IStudentRepository
	{
		public StudentRepository(DataContext context) : base(context)
		{
		}

		public async Task<StudentModel> AddTaskAsync(StudentModel student)
		{
			StudentModel addedStudent = (await _context.Students.AddAsync(student)).Entity;
			await _context.SaveChangesAsync();
			return addedStudent;
		}

		public async Task<bool> ExistsByEmailTaskAsync(string email) =>
			await _context.Students.AnyAsync(x => x.Email == email);

		public async Task<StudentModel> FindByIdTaskAsync(Guid id) =>
			await _context.Students.Where(x => x.StudentId == id).SingleOrDefaultAsync();

		public async Task<StudentModel> FindByEmailTaskAsync(string email) =>
			await _context.Students.SingleOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

		public async Task<List<StudentModel>> FindAllByCourseIdTaskAsync(Guid courseId) =>
			await _context.Students.Where(c => c.CourseId == courseId).ToListAsync();

		public async Task<StudentModel> UpdateTaskAsync(StudentModel student, StudentModel newStudent)
		{
			_context.Entry(student).CurrentValues.SetValues(newStudent);
			await _context.SaveChangesAsync();
			return newStudent;
		}

		public async Task DeleteAsync(StudentModel student)
		{
			_context.Students.Remove(student);
			await _context.SaveChangesAsync();
		}
	}
}