using CSGSI.Nodes;
using Newtonsoft.Json.Linq;

namespace CSGSI
{
    /// <summary>
    /// This object represents the entire game state
    /// </summary>
    public class GameState
    {
        private readonly JObject _data;

        private ProviderNode _provider;
        private MapNode _map;
        private RoundNode _round;
        private GrenadesNode _grenades;
        private PlayerNode _player;
        private AllPlayersNode _allPlayers;
        private BombNode _bomb;
        private PhaseCountdownsNode _phaseCountdowns;
        private GameState _previously;
        private GameState _added;
        private AuthNode _auth;

        /// <summary>
        /// Contains information about the game that is sending the data and the Steam user that is running the game itself.
        /// </summary>
        public ProviderNode Provider
        {
            get => _provider ??= new ProviderNode(_data["provider"]?.ToString() ?? "");
            set => _provider = value;
        }

        /// <summary>
        /// Contains information about the current map and match (i.e. match score and remaining timeouts)
        /// </summary>
        public MapNode Map => _map ??= new MapNode(_data["map"]?.ToString() ?? "");

        /// <summary>
        /// Contains information about the state of the current round (e.g. phase or the winning team)
        /// </summary>
        public RoundNode Round
        {
            get
            {
                if (_round != null)
                {
                    return _round;
                }

                var roundJson = _data["round"]?.ToString() ?? "{}";
                _round = new RoundNode(roundJson);

                return _round;
            }
        }

        /// <summary>
        /// Contains information about the grenades that currently exist.
        /// </summary>
        public GrenadesNode Grenades
        {
            get => _grenades ??= new GrenadesNode(_data["grenades"]?.ToString() ?? "");
            set => _grenades = value;
        }

        /// <summary>
        /// Contains information about the player (i.e. in the current POV, meaning this changes frequently during spectating)
        /// </summary>
        public PlayerNode Player
        {
            get => _player ??= new PlayerNode(_data["player"]?.ToString() ?? "");
            set => _player = value;
        }

        /// <summary>
        /// Contains information about all players.
        /// !! This node is only available when spectating the match with access to every players' POV !!
        /// </summary>
        public AllPlayersNode AllPlayers => _allPlayers ??= new AllPlayersNode(_data["allplayers"]?.ToString() ?? "");

        /// <summary>
        /// Contains information about the bomb.
        /// </summary>
        public BombNode Bomb => _bomb ??= new BombNode(_data["bomb"]?.ToString() ?? "");

        /// <summary>
        /// Contains information about the current "phase" that the round (e.g. bomb planted) is in and how long the phase is going to last.
        /// </summary>
        public PhaseCountdownsNode PhaseCountdowns
        {
            get => _phaseCountdowns ??= new PhaseCountdownsNode(_data["phase_countdowns"]?.ToString() ?? "");
            set => _phaseCountdowns = value;
        }

        /// <summary>
        /// When information has changed from the previous gamestate to the current one, the old values (before the change) are stored in this node.
        /// </summary>
        public GameState Previously => _previously ??= new GameState(_data["previously"]?.ToString() ?? "");

        /// <summary>
        /// When information has been received that was not present in the previous gamestate, the new values are (also) stored in this node.
        /// </summary>
        public GameState Added
        {
            get => _added ??= new GameState(_data["added"]?.ToString() ?? "");
            set => _added = value;
        }

        /// <summary>
        /// An auth code/phrase that can be set in your gamestate_integration_*.cfg.
        /// </summary>
        public AuthNode Auth
        {
            get => _auth ??= new AuthNode(_data["auth"]?.ToString() ?? "");
            set => _auth = value;
        }

        /// <summary>
        /// The JSON string that was used to generate this object
        /// </summary>
        public string Json { get; }

        /// <summary>
        /// Initialises a new GameState object using a JSON string
        /// </summary>
        /// <param name="jsonString"></param>
        public GameState(string jsonString)
        {
            if (jsonString.Equals(""))
            {
                jsonString = "{}";
            }

            Json = jsonString;
            _data = JObject.Parse(jsonString);
        }
    }
}