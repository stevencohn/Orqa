//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Gets or sets the StartMode of a named service.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 09-Nov-2005      New
//************************************************************************************************

namespace River.Orqa.Dialogs
{
	using System;
	using System.Runtime.InteropServices;
	using System.ServiceProcess;


	//********************************************************************************************
	// class ServiceManager
	//********************************************************************************************

	/// <summary>
	/// Gets or sets the StartMode of a named service.
	/// </summary>

	public class ServiceManager
	{
		#region class QueryServiceConfig

		[StructLayout(LayoutKind.Sequential)]
		private class ServiceConfig
		{
			[MarshalAs(UnmanagedType.U4)]
			public uint serviceType;

			[MarshalAs(UnmanagedType.U4)]
			public uint startType;

			[MarshalAs(UnmanagedType.U4)]
			public uint errorControl;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string binaryPathName;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string loadOrderGroup;

			[MarshalAs(UnmanagedType.U4)]
			public uint tagID;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string dependencies;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string serviceStartName;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string displayName;
		}

		#endregion class QueryServiceConfig

		#region Extern Declarations

		private const int SC_MANAGER_ALL_ACCESS = 0x000F003F;
		private const int SERVICE_CHANGE_CONFIG = 0x00000002;
		private const uint SERVICE_NO_CHANGE = 0xFFFFFFFF;
		private const int SERVICE_QUERY_CONFIG = 0x00000001;

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool ChangeServiceConfig (
			IntPtr hService, uint nServiceType, uint nStartType,
			uint nErrorControl, string lpBinaryPathName, string lpLoadOrderGroup,
			IntPtr lpdwTagId, string lpDependencies, string lpServiceStartName,
			string lpPassword, string lpDisplayName);
		
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool QueryServiceConfig (
			IntPtr hService, IntPtr intPtrQueryConfig, uint cbBufSize, 
			out uint pcbBytesNeeded);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr OpenSCManager (
			string lpMachineName, string lpDatabaseName, uint dwDesiredAccess);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr OpenService (
			IntPtr hSCManager, string lpServiceName, uint dwDesiredAccess);

		#endregion Extern Declarations

		private enum ErrorCode : int
		{
			#region enum
			NO_ERROR = 0,
			ERROR_SUCCESS = 0,
			ERROR_INVALID_FUNCTION = 1,
			ERROR_FILE_NOT_FOUND = 2,
			ERROR_PATH_NOT_FOUND = 3,
			ERROR_TOO_MANY_OPEN_FILES = 4,
			ERROR_ACCESS_DENIED = 5,
			ERROR_INVALID_HANDLE = 6,
			ERROR_ARENA_TRASHED = 7,
			ERROR_NOT_ENOUGH_MEMORY = 8,
			ERROR_INVALID_BLOCK = 9,
			ERROR_BAD_ENVIRONMENT = 10,
			ERROR_BAD_FORMAT = 11,
			ERROR_INVALID_ACCESS = 12,
			ERROR_INVALID_DATA = 13,
			ERROR_OUTOFMEMORY = 14,
			ERROR_INVALID_DRIVE = 15,
			ERROR_CURRENT_DIRECTORY = 16,
			ERROR_NOT_SAME_DEVICE = 17,
			ERROR_NO_MORE_FILES = 18,
			ERROR_WRITE_PROTECT = 19,
			ERROR_BAD_UNIT = 20,
			ERROR_NOT_READY = 21,
			ERROR_BAD_COMMAND = 22,
			ERROR_CRC = 23,
			ERROR_BAD_LENGTH = 24,
			ERROR_SEEK = 25,
			ERROR_NOT_DOS_DISK = 26,
			ERROR_SECTOR_NOT_FOUND = 27,
			ERROR_OUT_OF_PAPER = 28,
			ERROR_WRITE_FAULT = 29,
			ERROR_READ_FAULT = 30,
			ERROR_GEN_FAILURE = 31,
			ERROR_SHARING_VIOLATION = 32,
			ERROR_LOCK_VIOLATION = 33,
			ERROR_WRONG_DISK = 34,
			ERROR_SHARING_BUFFER_EXCEEDED = 36,
			ERROR_HANDLE_EOF = 38,
			ERROR_HANDLE_DISK_FULL = 39,
			ERROR_NOT_SUPPORTED = 50,
			ERROR_REM_NOT_LIST = 51,
			ERROR_DUP_NAME = 52,
			ERROR_BAD_NETPATH = 53,
			ERROR_NETWORK_BUSY = 54,
			ERROR_DEV_NOT_EXIST = 55,
			ERROR_TOO_MANY_CMDS = 56,
			ERROR_ADAP_HDW_ERR = 57,
			ERROR_BAD_NET_RESP = 58,
			ERROR_UNEXP_NET_ERR = 59,
			ERROR_BAD_REM_ADAP = 60,
			ERROR_PRINTQ_FULL = 61,
			ERROR_NO_SPOOL_SPACE = 62,
			ERROR_PRINT_CANCELLED = 63,
			ERROR_NETNAME_DELETED = 64,
			ERROR_NETWORK_ACCESS_DENIED = 65,
			ERROR_BAD_DEV_TYPE = 66,
			ERROR_BAD_NET_NAME = 67,
			ERROR_TOO_MANY_NAMES = 68,
			ERROR_TOO_MANY_SESS = 69,
			ERROR_SHARING_PAUSED = 70,
			ERROR_REQ_NOT_ACCEP = 71,
			ERROR_REDIR_PAUSED = 72,
			ERROR_FILE_EXISTS = 80,
			ERROR_CANNOT_MAKE = 82,
			ERROR_FAIL_I24 = 83,
			ERROR_OUT_OF_STRUCTURES = 84,
			ERROR_ALREADY_ASSIGNED = 85,
			ERROR_INVALID_PASSWORD = 86,
			ERROR_INVALID_PARAMETER = 87,
			ERROR_NET_WRITE_FAULT = 88,
			ERROR_NO_PROC_SLOTS = 89,
			ERROR_TOO_MANY_SEMAPHORES = 100,
			ERROR_EXCL_SEM_ALREADY_OWNED = 101,
			ERROR_SEM_IS_SET = 102,
			ERROR_TOO_MANY_SEM_REQUESTS = 103,
			ERROR_INVALID_AT_INTERRUPT_TIME = 104,
			ERROR_SEM_OWNER_DIED = 105,
			ERROR_SEM_USER_LIMIT = 106,
			ERROR_DISK_CHANGE = 107,
			ERROR_DRIVE_LOCKED = 108,
			ERROR_BROKEN_PIPE = 109,
			ERROR_OPEN_FAILED = 110,
			ERROR_BUFFER_OVERFLOW = 111,
			ERROR_DISK_FULL = 112,
			ERROR_NO_MORE_SEARCH_HANDLES = 113,
			ERROR_INVALID_TARGET_HANDLE = 114,
			ERROR_INVALID_CATEGORY = 117,
			ERROR_INVALID_VERIFY_SWITCH = 118,
			ERROR_BAD_DRIVER_LEVEL = 119,
			ERROR_CALL_NOT_IMPLEMENTED = 120,
			ERROR_SEM_TIMEOUT = 121,
			ERROR_INSUFFICIENT_BUFFER = 122,
			ERROR_INVALID_NAME = 123,
			ERROR_INVALID_LEVEL = 124,
			ERROR_NO_VOLUME_LABEL = 125,
			ERROR_MOD_NOT_FOUND = 126,
			ERROR_PROC_NOT_FOUND = 127,
			ERROR_WAIT_NO_CHILDREN = 128,
			ERROR_CHILD_NOT_COMPLETE = 129,
			ERROR_DIRECT_ACCESS_HANDLE = 130,
			ERROR_NEGATIVE_SEEK = 131,
			ERROR_SEEK_ON_DEVICE = 132,
			ERROR_IS_JOIN_TARGET = 133,
			ERROR_IS_JOINED = 134,
			ERROR_IS_SUBSTED = 135,
			ERROR_NOT_JOINED = 136,
			ERROR_NOT_SUBSTED = 137,
			ERROR_JOIN_TO_JOIN = 138,
			ERROR_SUBST_TO_SUBST = 139,
			ERROR_JOIN_TO_SUBST = 140,
			ERROR_SUBST_TO_JOIN = 141,
			ERROR_BUSY_DRIVE = 142,
			ERROR_SAME_DRIVE = 143,
			ERROR_DIR_NOT_ROOT = 144,
			ERROR_DIR_NOT_EMPTY = 145,
			ERROR_IS_SUBST_PATH = 146,
			ERROR_IS_JOIN_PATH = 147,
			ERROR_PATH_BUSY = 148,
			ERROR_IS_SUBST_TARGET = 149,
			ERROR_SYSTEM_TRACE = 150,
			ERROR_INVALID_EVENT_COUNT = 151,
			ERROR_TOO_MANY_MUXWAITERS = 152,
			ERROR_INVALID_LIST_FORMAT = 153,
			ERROR_LABEL_TOO_LONG = 154,
			ERROR_TOO_MANY_TCBS = 155,
			ERROR_SIGNAL_REFUSED = 156,
			ERROR_DISCARDED = 157,
			ERROR_NOT_LOCKED = 158,
			ERROR_BAD_THREADID_ADDR = 159,
			ERROR_BAD_ARGUMENTS = 160,
			ERROR_BAD_PATHNAME = 161,
			ERROR_SIGNAL_PENDING = 162,
			ERROR_MAX_THRDS_REACHED = 164,
			ERROR_LOCK_FAILED = 167,
			ERROR_BUSY = 170,
			ERROR_CANCEL_VIOLATION = 173,
			ERROR_ATOMIC_LOCKS_NOT_SUPPORTED = 174,
			ERROR_INVALID_SEGMENT_NUMBER = 180,
			ERROR_INVALID_ORDINAL = 182,
			ERROR_ALREADY_EXISTS = 183,
			ERROR_INVALID_FLAG_NUMBER = 186,
			ERROR_SEM_NOT_FOUND = 187,
			ERROR_INVALID_STARTING_CODESEG = 188,
			ERROR_INVALID_STACKSEG = 189,
			ERROR_INVALID_MODULETYPE = 190,
			ERROR_INVALID_EXE_SIGNATURE = 191,
			ERROR_EXE_MARKED_INVALID = 192,
			ERROR_BAD_EXE_FORMAT = 193,
			ERROR_ITERATED_DATA_EXCEEDS_64K = 194,
			ERROR_INVALID_MINALLOCSIZE = 195,
			ERROR_DYNLINK_FROM_INVALID_RING = 196,
			ERROR_IOPL_NOT_ENABLED = 197,
			ERROR_INVALID_SEGDPL = 198,
			ERROR_AUTODATASEG_EXCEEDS_64KB = 199,
			ERROR_RING2SEG_MUST_BE_MOVABLE = 200,
			ERROR_RELOC_CHAIN_XEEDS_SEGLIM = 201,
			ERROR_INFLOOP_IN_RELOC_CHAIN = 202,
			ERROR_ENVVAR_NOT_FOUND = 203,
			ERROR_NO_SIGNAL_SENT = 205,
			ERROR_FILENAME_EXCED_RANGE = 206,
			ERROR_RING2_STACK_IN_USE = 207,
			ERROR_META_EXPANSION_TOO_LONG = 208,
			ERROR_INVALID_SIGNAL_NUMBER = 209,
			ERROR_THREAD_1_INACTIVE = 210,
			ERROR_LOCKED = 212,
			ERROR_TOO_MANY_MODULES = 214,
			ERROR_NESTING_NOT_ALLOWED = 215,
			ERROR_BAD_PIPE = 230,
			ERROR_PIPE_BUSY = 231,
			ERROR_NO_DATA = 232,
			ERROR_PIPE_NOT_CONNECTED = 233,
			ERROR_MORE_DATA = 234,
			ERROR_VC_DISCONNECTED = 240,
			ERROR_INVALID_EA_NAME = 254,
			ERROR_EA_LIST_INCONSISTENT = 255,
			ERROR_NO_MORE_ITEMS = 259,
			ERROR_CANNOT_COPY = 266,
			ERROR_DIRECTORY = 267,
			ERROR_EAS_DIDNT_FIT = 275,
			ERROR_EA_FILE_CORRUPT = 276,
			ERROR_EA_TABLE_FULL = 277,
			ERROR_INVALID_EA_HANDLE = 278,
			ERROR_EAS_NOT_SUPPORTED = 282,
			ERROR_NOT_OWNER = 288,
			ERROR_TOO_MANY_POSTS = 298,
			ERROR_MR_MID_NOT_FOUND = 317,
			ERROR_INVALID_ADDRESS = 487,
			ERROR_ARITHMETIC_OVERFLOW = 534,
			ERROR_PIPE_CONNECTED = 535,
			ERROR_PIPE_LISTENING = 536,
			ERROR_EA_ACCESS_DENIED = 994,
			ERROR_OPERATION_ABORTED = 995,
			ERROR_IO_INCOMPLETE = 996,
			ERROR_IO_PENDING = 997,
			ERROR_NOACCESS = 998,
			ERROR_SWAPERROR = 999,
			ERROR_STACK_OVERFLOW = 1001,
			ERROR_INVALID_MESSAGE = 1002,
			ERROR_CAN_NOT_COMPLETE = 1003,
			ERROR_INVALID_FLAGS = 1004,
			ERROR_UNRECOGNIZED_VOLUME = 1005,
			ERROR_FILE_INVALID = 1006,
			ERROR_FULLSCREEN_MODE = 1007,
			ERROR_NO_TOKEN = 1008,
			ERROR_BADDB = 1009,
			ERROR_BADKEY = 1010,
			ERROR_CANTOPEN = 1011,
			ERROR_CANTREAD = 1012,
			ERROR_CANTWRITE = 1013,
			ERROR_REGISTRY_RECOVERED = 1014,
			ERROR_REGISTRY_CORRUPT = 1015,
			ERROR_REGISTRY_IO_FAILED = 1016,
			ERROR_NOT_REGISTRY_FILE = 1017,
			ERROR_KEY_DELETED = 1018,
			ERROR_NO_LOG_SPACE = 1019,
			ERROR_KEY_HAS_CHILDREN = 1020,
			ERROR_CHILD_MUST_BE_VOLATILE = 1021,
			ERROR_NOTIFY_ENUM_DIR = 1022,
			ERROR_DEPENDENT_SERVICES_RUNNING = 1051,
			ERROR_INVALID_SERVICE_CONTROL = 1052,
			ERROR_SERVICE_REQUEST_TIMEOUT = 1053,
			ERROR_SERVICE_NO_THREAD = 1054,
			ERROR_SERVICE_DATABASE_LOCKED = 1055,
			ERROR_SERVICE_ALREADY_RUNNING = 1056,
			ERROR_INVALID_SERVICE_ACCOUNT = 1057,
			ERROR_SERVICE_DISABLED = 1058,
			ERROR_CIRCULAR_DEPENDENCY = 1059,
			ERROR_SERVICE_DOES_NOT_EXIST = 1060,
			ERROR_SERVICE_CANNOT_ACCEPT_CTRL = 1061,
			ERROR_SERVICE_NOT_ACTIVE = 1062,
			ERROR_FAILED_SERVICE_CONTROLLER_CONNECT = 1063,
			ERROR_EXCEPTION_IN_SERVICE = 1064,
			ERROR_DATABASE_DOES_NOT_EXIST = 1065,
			ERROR_SERVICE_SPECIFIC_ERROR = 1066,
			ERROR_PROCESS_ABORTED = 1067,
			ERROR_SERVICE_DEPENDENCY_FAIL = 1068,
			ERROR_SERVICE_LOGON_FAILED = 1069,
			ERROR_SERVICE_START_HANG = 1070,
			ERROR_INVALID_SERVICE_LOCK = 1071,
			ERROR_SERVICE_MARKED_FOR_DELETE = 1072,
			ERROR_SERVICE_EXISTS = 1073,
			ERROR_ALREADY_RUNNING_LKG = 1074,
			ERROR_SERVICE_DEPENDENCY_DELETED = 1075,
			ERROR_BOOT_ALREADY_ACCEPTED = 1076,
			ERROR_SERVICE_NEVER_STARTED = 1077,
			ERROR_DUPLICATE_SERVICE_NAME = 1078,
			ERROR_END_OF_MEDIA = 1100,
			ERROR_FILEMARK_DETECTED = 1101,
			ERROR_BEGINNING_OF_MEDIA = 1102,
			ERROR_SETMARK_DETECTED = 1103,
			ERROR_NO_DATA_DETECTED = 1104,
			ERROR_PARTITION_FAILURE = 1105,
			ERROR_INVALID_BLOCK_LENGTH = 1106,
			ERROR_DEVICE_NOT_PARTITIONED = 1107,
			ERROR_UNABLE_TO_LOCK_MEDIA = 1108,
			ERROR_UNABLE_TO_UNLOAD_MEDIA = 1109,
			ERROR_MEDIA_CHANGED = 1110,
			ERROR_BUS_RESET = 1111,
			ERROR_NO_MEDIA_IN_DRIVE = 1112,
			ERROR_NO_UNICODE_TRANSLATION = 1113,
			ERROR_DLL_INIT_FAILED = 1114,
			ERROR_SHUTDOWN_IN_PROGRESS = 1115,
			ERROR_NO_SHUTDOWN_IN_PROGRESS = 1116,
			ERROR_IO_DEVICE = 1117,
			ERROR_SERIAL_NO_DEVICE = 1118,
			ERROR_IRQ_BUSY = 1119,
			ERROR_MORE_WRITES = 1120,
			ERROR_COUNTER_TIMEOUT = 1121,
			ERROR_FLOPPY_ID_MARK_NOT_FOUND = 1122,
			ERROR_FLOPPY_WRONG_CYLINDER = 1123,
			ERROR_FLOPPY_UNKNOWN_ERROR = 1124,
			ERROR_FLOPPY_BAD_REGISTERS = 1125,
			ERROR_DISK_RECALIBRATE_FAILED = 1126,
			ERROR_DISK_OPERATION_FAILED = 1127,
			ERROR_DISK_RESET_FAILED = 1128,
			ERROR_EOM_OVERFLOW = 1129,
			ERROR_NOT_ENOUGH_SERVER_MEMORY = 1130,
			ERROR_POSSIBLE_DEADLOCK = 1131,
			ERROR_MAPPED_ALIGNMENT = 1132
			#endregion
		}

		private string hostName;
		private IntPtr handle;


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostName"></param>

		public ServiceManager (string hostName)
		{
			this.hostName = hostName;
		}


		//========================================================================================
		// Open()
		//========================================================================================

		/// <summary>
		/// 
		/// </summary>

		public void Open ()
		{
			handle = OpenSCManager(hostName, null, SC_MANAGER_ALL_ACCESS);

			if (handle == IntPtr.Zero)
			{
				int error = Marshal.GetLastWin32Error();

				switch (error)
				{
					case (int)ErrorCode.ERROR_ACCESS_DENIED:
						throw new ExternalException("Error opening service manager, access denied");

					case (int)ErrorCode.ERROR_DATABASE_DOES_NOT_EXIST:
						throw new ExternalException("Error opening service manager, database does not exists");

					case (int)ErrorCode.ERROR_INVALID_PARAMETER:
						throw new ExternalException("Error opening service manager, invalid parameter");

					default:
						throw new ExternalException("Error opening service manager [" + error + "]");
				}
			}
		}


		//========================================================================================
		// GetServiceStartMode()
		//========================================================================================

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceName"></param>
		/// <returns></returns>

		public ServiceStartMode GetServiceStartMode (string serviceName)
		{
			if (handle == IntPtr.Zero)
				throw new ExternalException("GetServiceStartMode empty handle");

			IntPtr serviceHandle = OpenService(handle, serviceName, SERVICE_QUERY_CONFIG);
			if (serviceHandle == IntPtr.Zero)
				throw new ExternalException("Error querying service");

			uint bytesNeeded = 0;

			// Allocate memory for struct.
			IntPtr ptr = Marshal.AllocHGlobal(4096);

			bool success = QueryServiceConfig(serviceHandle, ptr, 4096, out bytesNeeded);

			ServiceConfig config = new ServiceConfig();

			// Copy to buffer and free memory for struct
			Marshal.PtrToStructure(ptr, config);
			Marshal.FreeHGlobal(ptr); 

			ServiceStartMode startMode = (ServiceStartMode)config.startType;
			return startMode;
		}


		//========================================================================================
		// SetServiceStartMode()
		//========================================================================================

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceName"></param>
		/// <param name="mode"></param>

		public bool SetServiceStartMode (string serviceName, ServiceStartMode mode)
		{
			if (handle == IntPtr.Zero)
				throw new ExternalException("SetServiceStartMode empty handle");

			IntPtr serviceHandle = OpenService(handle, serviceName, SERVICE_CHANGE_CONFIG);
			if (serviceHandle == IntPtr.Zero)
				throw new ExternalException("Error querying service");

			bool success = ChangeServiceConfig(serviceHandle,
				SERVICE_NO_CHANGE,					// uint nServiceType
				(uint)mode,							// uint nStartType
				SERVICE_NO_CHANGE,					// uint nErrorControl
				null,								// string lpBinaryPathName
				null,								// string lpLoadOrderGroup,
				(IntPtr)null,						// IntPtr lpdwTagId,
				null,								// string lpDependencies
				null,								// string lpServiceStartName
				null,								// string lpPassword
				null);								// string lpDisplayName

			return success;
		}
	}
}