/*
 * Thresher IRC client library
 * Copyright (C) 2002 Aaron Hunter <thresher@sharkbite.org>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
 * 
 * See the gpl.txt file located in the top-level-directory of
 * the archive of this library for complete text of license.
*/

#if DEBUG

using System;
using System.Net;
using Sharkbite.Irc;
using NUnit.Framework;


namespace Sharkbite.Irc.Test
{
	/// <summary>
	/// Test DccChatTest.
	/// </summary>
	[TestFixture]
	public class DccChatSessionTest
	{


		[SetUp]
		public void SetUp() 
		{
		}

		[Test]
		public void TestDummy() 
		{
			Assertion.Assert( true );
		}

	}
}
#endif