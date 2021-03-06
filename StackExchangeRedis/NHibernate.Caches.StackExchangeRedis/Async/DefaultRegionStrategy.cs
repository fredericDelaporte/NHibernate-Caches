﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NHibernate.Cache;
using StackExchange.Redis;
using static NHibernate.Caches.StackExchangeRedis.ConfigurationHelper;

namespace NHibernate.Caches.StackExchangeRedis
{
	using System.Threading.Tasks;
	public partial class DefaultRegionStrategy : AbstractRegionStrategy
	{

		/// <inheritdoc />
		public override Task<object> GetAsync(object key, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<object>(cancellationToken);
			}
			return GetAsync(key, 0, cancellationToken);
		}

		private async Task<object> GetAsync(object key, int retries, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				return await (base.GetAsync(key, cancellationToken)).ConfigureAwait(false);
			}
			catch (RedisServerException e) when (e.Message == InvalidVersionMessage)
			{
				Log.Debug("Version '{0}' is not valid anymore, updating version...", CurrentVersion);
				await (InitializeVersionAsync(cancellationToken)).ConfigureAwait(false);
				if (retries >= _retryTimes)
				{
					Log.Warn("Unable to perform '{0}' operation due to concurrent clear operations, total retries: '{1}'.", nameof(GetAsync), retries);
					return null;
				}

				if (Log.IsDebugEnabled())
				{
					Log.Debug("Retry to fetch the object with key: '{0}'", CurrentVersion, GetCacheKey(key));
				}

				return await (GetAsync(key, retries + 1, cancellationToken)).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		public override Task<object[]> GetManyAsync(object[] keys, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<object[]>(cancellationToken);
			}
			return GetManyAsync(keys, 0, cancellationToken);
		}

		private async Task<object[]> GetManyAsync(object[] keys, int retries, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				return await (base.GetManyAsync(keys, cancellationToken)).ConfigureAwait(false);
			}
			catch (RedisServerException e) when (e.Message == InvalidVersionMessage)
			{
				Log.Debug("Version '{0}' is not valid anymore, updating version...", CurrentVersion);
				await (InitializeVersionAsync(cancellationToken)).ConfigureAwait(false);
				if (retries >= _retryTimes)
				{
					Log.Warn("Unable to perform '{0}' operation due to concurrent clear operations, total retries: '{1}'.", nameof(GetManyAsync), retries);
					return new object[keys.Length];
				}

				if (Log.IsDebugEnabled())
				{
					Log.Debug("Retry to fetch objects with keys: {0}",
						CurrentVersion,
						string.Join(",", keys.Select(o => $"'{GetCacheKey(o)}'")));
				}

				return await (GetManyAsync(keys, retries + 1, cancellationToken)).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		public override Task<string> LockAsync(object key, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<string>(cancellationToken);
			}
			return LockAsync(key, 0, cancellationToken);
		}

		private async Task<string> LockAsync(object key, int retries, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				return await (base.LockAsync(key, cancellationToken)).ConfigureAwait(false);
			}
			catch (RedisServerException e) when (e.Message == InvalidVersionMessage)
			{
				Log.Debug("Version '{0}' is not valid anymore, updating version...", CurrentVersion);
				await (InitializeVersionAsync(cancellationToken)).ConfigureAwait(false);
				if (retries >= _retryTimes)
				{
					throw new CacheException(
						$"Unable to perform {nameof(LockAsync)} operation due to concurrent clear operations, total retries: '{retries}'.");
				}

				if (Log.IsDebugEnabled())
				{
					Log.Debug("Retry to lock the object with key: '{0}'", CurrentVersion, GetCacheKey(key));
				}

				return await (LockAsync(key, retries + 1, cancellationToken)).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		public override Task<string> LockManyAsync(object[] keys, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<string>(cancellationToken);
			}
			return LockManyAsync(keys, 0, cancellationToken);
		}

		private async Task<string> LockManyAsync(object[] keys, int retries, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				return await (base.LockManyAsync(keys, cancellationToken)).ConfigureAwait(false);
			}
			catch (RedisServerException e) when (e.Message == InvalidVersionMessage)
			{
				Log.Debug("Version '{0}' is not valid anymore, updating version...", CurrentVersion);
				await (InitializeVersionAsync(cancellationToken)).ConfigureAwait(false);
				if (retries >= _retryTimes)
				{
					throw new CacheException(
						$"Unable to perform {nameof(LockManyAsync)} operation due to concurrent clear operations, total retries: '{retries}'.");
				}

				if (Log.IsDebugEnabled())
				{
					Log.Debug("Retry to lock objects with keys: {0}",
						CurrentVersion,
						string.Join(",", keys.Select(o => $"'{GetCacheKey(o)}'")));
				}

				return await (LockManyAsync(keys, retries + 1, cancellationToken)).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		public override async Task PutAsync(object key, object value, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				await (base.PutAsync(key, value, cancellationToken)).ConfigureAwait(false);
			}
			catch (RedisServerException e) when (e.Message == InvalidVersionMessage)
			{
				Log.Debug("Version '{0}' is not valid anymore, updating version...", CurrentVersion);
				await (InitializeVersionAsync(cancellationToken)).ConfigureAwait(false);
				// Here we don't know if the operation was executed after as successful lock, so
				// the easiest solution is to skip the operation
			}
		}

		/// <inheritdoc />
		public override async Task PutManyAsync(object[] keys, object[] values, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				await (base.PutManyAsync(keys, values, cancellationToken)).ConfigureAwait(false);
			}
			catch (RedisServerException e) when (e.Message == InvalidVersionMessage)
			{
				Log.Debug("Version '{0}' is not valid anymore, updating version...", CurrentVersion);
				await (InitializeVersionAsync(cancellationToken)).ConfigureAwait(false);
				// Here we don't know if the operation was executed after as successful lock, so
				// the easiest solution is to skip the operation
			}
		}

		/// <inheritdoc />
		public override async Task<bool> RemoveAsync(object key, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				return await (base.RemoveAsync(key, cancellationToken)).ConfigureAwait(false);
			}
			catch (RedisServerException e) when (e.Message == InvalidVersionMessage)
			{
				Log.Debug("Version '{0}' is not valid anymore, updating version...", CurrentVersion);
				await (InitializeVersionAsync(cancellationToken)).ConfigureAwait(false);
				// There is no point removing the key in the new version.
				return false;
			}
		}

		/// <inheritdoc />
		public override async Task<bool> UnlockAsync(object key, string lockValue, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				return await (base.UnlockAsync(key, lockValue, cancellationToken)).ConfigureAwait(false);
			}
			catch (RedisServerException e) when (e.Message == InvalidVersionMessage)
			{
				Log.Debug("Version '{0}' is not valid anymore, updating version...", CurrentVersion);
				await (InitializeVersionAsync(cancellationToken)).ConfigureAwait(false);
				// If the lock was acquired in the old version we are unable to unlock the key.
				return false;
			}
		}

		/// <inheritdoc />
		public override async Task<int> UnlockManyAsync(object[] keys, string lockValue, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				return await (base.UnlockManyAsync(keys, lockValue, cancellationToken)).ConfigureAwait(false);
			}
			catch (RedisServerException e) when (e.Message == InvalidVersionMessage)
			{
				Log.Debug("Version '{0}' is not valid anymore, updating version...", CurrentVersion);
				await (InitializeVersionAsync(cancellationToken)).ConfigureAwait(false);
				// If the lock was acquired in the old version we are unable to unlock the keys.
				return 0;
			}
		}

		/// <inheritdoc />
		public override async Task ClearAsync(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			Log.Debug("Clearing region: '{0}'.", RegionKey);
			await (_versionLock.WaitAsync(cancellationToken)).ConfigureAwait(false);
			try
			{
				cancellationToken.ThrowIfCancellationRequested();
				var results = (RedisValue[]) await (Database.ScriptEvaluateAsync(UpdateVersionLuaScript,
					_regionKeyArray, _maxVersionArray)).ConfigureAwait(false);
				var version = (long) results[0];
				UpdateVersion(version);
				if (_usePubSub)
				{
					cancellationToken.ThrowIfCancellationRequested();
					await (ConnectionMultiplexer.GetSubscriber().PublishAsync(RegionKey, version + ";" + _databaseIndex)).ConfigureAwait(false);
				}
			}
			finally
			{
				_versionLock.Release();
			}
		}

		private async Task InitializeVersionAsync(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await (_versionLock.WaitAsync(cancellationToken)).ConfigureAwait(false);
			try
			{
				cancellationToken.ThrowIfCancellationRequested();
				var results = (RedisValue[]) await (Database.ScriptEvaluateAsync(InitializeVersionLuaScript, _regionKeyArray)).ConfigureAwait(false);
				UpdateVersion((long) results[0]);
			}
			finally
			{
				_versionLock.Release();
			}
		}
	}
}
