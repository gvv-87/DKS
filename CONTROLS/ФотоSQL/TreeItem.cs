namespace ДКС
{
    public enum NodeType { Album, Photo };

	/// <summary>
	/// This is a helper class to contain information about	treeview items
	/// </summary>
	public class TreeItem	 //------------------------------------
	{
		private int			m_nId        = 0;
		private NodeType	m_Type       = NodeType.Photo;

		public TreeItem(NodeType type, int nId)
		{
			m_Type    = type;
			m_nId     = nId;
		}

		public NodeType Type	{	get{ return m_Type; } }
		public int Id			{	get{ return m_nId; } }
		
	}
}
